using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.ServiceModel;
using ClassEngine;

namespace NormalizerMESSAGING
{
    class NormalizerMESSAGINGApp
    {

        private const string QUEUE_NAME = "normalizerMESSAGING_Queue";
        private const string EXCHANGE_NAME = "aggregator_exchange";

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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    BankEngine be = new BankEngine();
                    be.sendMessageCorrelationId(EXCHANGE_NAME, message, message, QUEUE_NAME);
                    //be.sendMessageReplyAddress(EXCHANGE_NAME, message, message, QUEUE_NAME);
                };

                channel.BasicConsume(queue: QUEUE_NAME,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}