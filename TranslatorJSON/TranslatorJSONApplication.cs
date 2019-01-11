using System;
using System.Text;
using Newtonsoft.Json;
using ClassEngine;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TranslatorJSON
{
    class TranslatorJSONApplication
    {
        private const string QUEUE_NAME = "translatorJSON_queue";
        private const string EXCHANGE_NAME = "recipientList_exchange";

        static void Main(string[] args)
        {
            Console.Title = "TranslatorJSON";

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

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);
                channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "JSON");

                Console.WriteLine(" TranslatorJSON ready ");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    string[] messageArray = message.Split('#');
                    string ssn = messageArray[0];
                    double amount = Convert.ToDouble(messageArray[1]);
                    int duration = Convert.ToInt32(messageArray[2]);
                    int creditScore = Convert.ToInt32(messageArray[3]);
                    Bank bank = JsonConvert.DeserializeObject<Bank>(messageArray[4]);

                    Console.WriteLine(" [x] Received {0}", message);

                    // Creating JSON request
                    JsonRequest json = new JsonRequest(ssn.Replace("-", ""), creditScore, amount, duration);

                    string println = "Loan request sent to " + bank.Bankname;
                    string replyAddress = bank.Normalizer;

                    BankEngine be = new BankEngine();
                    try
                    {
                        be.sendMessageReplyAddress(bank.Exchange, JsonConvert.SerializeObject(json), println, replyAddress);

                    } catch (Exception e)
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

