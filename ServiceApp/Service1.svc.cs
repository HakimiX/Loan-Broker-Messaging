using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ClassEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        private double baseRate = 4.0;

        private const string QUEUE_NAME = "loanRequest_queue";
        private Boolean received = false;
        private string messageReceived = "";

        // Test Method
        public int getAge(int age)
        {
            age = 12;
            return age;
        }

        // All Banks
        public List<Bank> getBanks(int creditScore, double amount, int duration)
        {
            Bank bank1 = new Bank(12345, "Danske Bank", "cphbusiness.bankXML", "normalizerXML_queue", "XML");
            Bank bank2 = new Bank(56789, "Nykredit Bank", "cphbusiness.bankJSON", "normalizerJSON_queue", "JSON");
            Bank bank3 = new Bank(01234, "Lunar Way", "LunarWay_exchange", "normalizerMESSAGING_queue", "MSG");
            Bank bank4 = new Bank(45678, "Spar Nord", "SparNord_exchange", "normalizerWS_queue", "WS");
            List<Bank> banks = new List<Bank>();

            
            // Configure rulebase
            if ((amount >= (double)50000) && (creditScore >= 40) && (duration <= 100))
            {
                banks.Add(bank1);
                banks.Add(bank2);
                banks.Add(bank3);
                banks.Add(bank4);

            }
            else
                 if ((amount >= (double)75000) && (amount <= (double)100000) && (creditScore >= 550) && (duration <= 150))
            {
                banks.Add(bank1);
                banks.Add(bank2);

            }
            else
                if ((amount <= (double)250000) && (creditScore >= 500) && (duration <= 200))
            {
                banks.Add(bank3);
                banks.Add(bank4);
            }
            else
            {
                banks.Add(bank1);
                banks.Add(bank2);
                banks.Add(bank3);
                banks.Add(bank4);
            }

            return banks;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void getInterestRate(string ssn, double amount, int duration, int creditScore, string replyToAddress)
        {
            double interestRate = baseRate + ((800 - creditScore) / 100);

            if (amount < 2000)
            {
                interestRate += 0.3;
            }
            else if (amount > 50000)
            {
                interestRate += 0.18;
            }
            else
            {
                interestRate += 0.6;
            }

            if (duration > 120)
            {
                interestRate += 0.6;
            }
            else if (duration < 60)
            {
                interestRate += 0.3;
            }
            else
            {
                interestRate += 0.4;
            }

            string messageToSend = ssn + "#" + interestRate;

            this.ResponseMessageToQueue(messageToSend, "normalizerWS_queue");
        }

        // Test Method
        public string getName(string name)
        {
            name = "hej";
            return name;
        }

        public void sendMessage(string EXCHANGE_NAME, string message)
        {
            // Set Connection Info
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            // Create Connection and Channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Publish Message
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);
                channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: "", basicProperties: null, body: body);

            }
        }


        public string LoanRequest(string ssn, double amount, int duration)
        {

            bool received = false;
            string messageReceived = "";

            string messageToSend = ssn + "#" + amount + "#" + duration;
            sendMessage("loanRequest_exchange", messageToSend);

            ConnectionFactory factory = new ConnectionFactory()

            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel1 = connection.CreateModel())
            {

                channel1.QueueDeclare(queue: "loanResponse_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel1.ExchangeDeclare("loanResponse_exchange", ExchangeType.Topic);
                channel1.QueueBind("loanResponse_queue", "loanResponse_exchange", "");

                Console.WriteLine(" LoanRequester ready ");

                var consumer1 = new EventingBasicConsumer(channel1);
                consumer1.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    received = true;
                    messageReceived = message;
                };

                channel1.BasicConsume(queue: "loanResponse_queue", autoAck: true, consumer: consumer1);

                while (!received)
                {
                }

                return messageReceived;
            }
        }


        public void ResponseMessageToQueue(string message, string replyAddress)
        {
            // Set Connection Info
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            // Create Connection and Channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Publish Message
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: replyAddress, basicProperties: null, body: body);
            }
        }
    }

 }
