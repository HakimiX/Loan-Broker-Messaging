using ClassEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCreditScore
{
    class GetCreditScoreProgram
    {
        private const string QUEUE_NAME = "loanRequest_queue";
        private const string EXCHANGE_NAME = "loanRequest_exchange";

        static void Main(string[] args)
        {
            Console.Title = "GetCreditScore";

            BankEngine be = new BankEngine();

            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);
                channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "");

                Console.WriteLine(" GetCreditScore ready ");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    string[] messageArray = message.Split('#');
                    string ssn = messageArray[0];
                    double amount = Convert.ToDouble(messageArray[1]);
                    int duration = Convert.ToInt32(messageArray[2]);

                    Console.WriteLine(" [x] Received {0}", "SSN:" + ssn + " AMOUNT:" + amount + " DURATION:" + duration);

                    int creditScore = GetCreditScore(ssn);

                    string messageToSend = ssn + "#" + amount + "#" + duration + "#" + creditScore;
                    string println = "SSN:" + ssn + " AMOUNT:" + amount + " DURATION:" + duration + " CREDITSCORE:" + creditScore;
                    be.sendMessage("creditScore_exchange", messageToSend, println);

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static int GetCreditScore(string ssn)
        {
            int creditScore = 0;

            CreditScoreServices.CreditScoreServiceClient creditBureau = new CreditScoreServices.CreditScoreServiceClient();
            creditScore = creditBureau.creditScore(ssn);

            return creditScore;
        }




    }
}
