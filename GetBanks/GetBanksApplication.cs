using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using ClassEngine;
using ServiceApp;

namespace GetBanks
{
    class GetBanksProgram
    {

        private const string QUEUE_NAME = "creditScore_queue";
        private const string EXCHANGE_NAME = "creditScore_exchange";

        public static void Main(string[] args)
        {
            Console.Title = "GetBanks";

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

                Console.WriteLine(" GetBanks ready ");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    string[] messageArray = message.Split('#');
                    string ssn = messageArray[0];
                    double amount = Convert.ToDouble(messageArray[1]);
                    int duration = Convert.ToInt32(messageArray[2]);
                    int creditScore = Convert.ToInt32(messageArray[3]);

                    List<Bank> listBanks = GetBanks(creditScore, amount, duration);

                    string messageToSend = ssn + "#" + amount + "#" + duration + "#" + creditScore + "#" + JsonConvert.SerializeObject(listBanks);
                    string println = "SSN:" + ssn + " AMOUNT:" + amount + " DURATION:" + duration + " CREDITSCORE:" + creditScore + " BANKS:{" + BankNamesToString(listBanks) + "}";
                    be.sendMessage("getBanks_exchange", messageToSend, println);

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static List<Bank> GetBanks(int creditScore, double amount, int duration)
        {
            Service1 service = new Service1();

            List<Bank> listBanks = new List<Bank>();

            foreach (var bank in service.getBanks(creditScore, amount, duration))
            {
                listBanks.Add(new Bank(bank.BankIdentificationCode, bank.Bankname, bank.Exchange, bank.Normalizer, bank.TranslatorType));
            }

            return listBanks;
        }



        private static string BankNamesToString(List<Bank> listBanks)
        {
            string bankNames = "";
            foreach (Bank bank in listBanks)
            {
                bankNames += bank.Bankname + ",";
            }

            return bankNames.TrimEnd(',');
        }



    }
}
