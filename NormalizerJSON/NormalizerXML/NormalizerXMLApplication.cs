using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ClassEngine;
using System;
using System.Text;
using System.Xml;

namespace NormalizerXML
{
    class NormalizerXMLApplication
    {
        private const string QUEUE_NAME = "normalizerXML_queue";
        private const string EXCHANGE_NAME = "aggregator_exchange";
       
        public static string normalizeSsn(string message)
        {
            String[] messageArr = message.Split('#');
            string ssn = messageArr[0];
            string interestRate = messageArr[1];
            if (ssn.Length != 11 && !ssn.Contains("-"))
            {
                ssn = ssn.Insert(6, "-");
            }

            string messageToSend = ssn + "#" + interestRate;

            return messageToSend;
        }

        static void Main(string[] args)
        {
            Console.Title = "NormalizerXML";

            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QUEUE_NAME,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                Console.WriteLine(" NormalizerXML ready ");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(message);
                    XmlNode loanResponseXML = doc.SelectSingleNode("LoanResponse");
                    string ssn = loanResponseXML["ssn"].InnerText.Insert(6, "-");
                    double interestRate = XmlConvert.ToDouble(loanResponseXML["interestRate"].InnerText);
                    string messageToSend = ssn + "#" + interestRate;

                    BankEngine be = new BankEngine();

                    messageToSend = normalizeSsn(messageToSend);

                    be.sendMessageCorrelationId(EXCHANGE_NAME, messageToSend, messageToSend, QUEUE_NAME);

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
