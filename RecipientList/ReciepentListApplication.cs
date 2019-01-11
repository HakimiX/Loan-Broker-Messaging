using ClassEngine;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipientList
{
    class RecipientListProgram
    {
        private const string QUEUE_NAME = "getBanks_queue";
        private const string EXCHANGE_NAME = "getBanks_exchange";

        static void Main(string[] args)
        {
            Console.Title = "Recipient List";
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

                Console.WriteLine(" RecipientList ready ");

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
                    List<Bank> listBanks = JsonConvert.DeserializeObject<List<Bank>>(messageArray[4]);
                    Console.WriteLine(" [x] Received {0}", "SSN:" + ssn + " AMOUNT:" + amount + " DURATION:" + duration + " CREDITSCORE:" + creditScore + " BANKS:{" + BankNamesToString(listBanks) + "}");

                    be.sendMessage("aggregatorList_exchange", ssn + "#" + messageArray[4], "Liste med " + listBanks.Count + " Bank Send To Aggregator ");

                    foreach (Bank bank in listBanks)
                    {
                        string messageSend = ssn + "#" + amount + "#" + duration + "#" + creditScore + "#" + JsonConvert.SerializeObject(bank);
                        string println = bank.Bankname;
                        string routingKey = bank.TranslatorType;

                        be.sendMessage("recipientList_exchange", messageSend, println, routingKey);
                    }



                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
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
