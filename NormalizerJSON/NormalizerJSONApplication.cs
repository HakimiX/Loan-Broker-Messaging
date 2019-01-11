using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ClassEngine;
using System.Text;
using Newtonsoft.Json;


namespace NormalizerJSON
{
    public class NormalizerJSONApplication
    {

        private const string QUEUE_NAME = "normalizerJSON_queue";
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

        public static void Main(string[] args)
        {
            Console.Title = "NormalizerJSON";


            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare();
                channel.QueueDeclare(queue: QUEUE_NAME,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                Console.WriteLine(" NormalizerJSON ready ");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    jsonResponse jsonResponse = JsonConvert.DeserializeObject<jsonResponse>(message);
                    string messageToSend = jsonResponse.ssn + "#" + jsonResponse.interestRate;
                    messageToSend = normalizeSsn(messageToSend);

                    BankEngine be = new BankEngine();
                    try
                    {
                        be.sendMessageCorrelationId(EXCHANGE_NAME, messageToSend, messageToSend, QUEUE_NAME);

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
