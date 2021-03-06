﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using ClassEngine;
using ServiceApp;

namespace TranslatorWS
{
    class TranslatorWSApplication
    {
        private const string QUEUE_NAME = "translatorWS_queue";
        private const string EXCHANGE_NAME = "recipientList_exchange";

        static void Main(string[] args)
        {
            Console.Title = "TranslatorWS";

            Service1 service = new Service1();

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
                channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "WS");

                Console.WriteLine(" TranslatorWS ready ");

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

                    string println = "Loan request sent to " + bank.Bankname;
                    string replyAddress = bank.Normalizer;

                    service.getInterestRate(ssn, amount, duration, creditScore, replyAddress);
                    Console.WriteLine(" [x] Sent {0}", println);

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
