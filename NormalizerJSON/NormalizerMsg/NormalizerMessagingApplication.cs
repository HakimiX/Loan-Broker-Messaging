using System;
using System.Text;
using ClassEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NormalizerMsg
{
    class NormalizerMESSAGINGApplication
    {
        private const string QUEUE_NAME = "normalizerMESSAGING_queue";
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
            Console.Title = "NormalizerMESSAGING";

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

                Console.WriteLine(" NormalizerMESSAGING ready ");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    message = normalizeSsn(message);

                    BankEngine be = new BankEngine();
                    try
                    {
                        be.sendMessageCorrelationId(EXCHANGE_NAME, message, message, QUEUE_NAME);

                    }
                    catch (Exception e)
                    {
                        e.GetBaseException();
                    }

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
