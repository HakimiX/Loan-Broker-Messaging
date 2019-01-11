using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Xml.Linq;
using ClassEngine;

namespace TranslatorXML
{
    class TranslatorXMLApplication
    {
        private const string QUEUE_NAME = "translatorXML_queue";
        private const string EXCHANGE_NAME = "recipientList_exchange";

        static void Main(string[] args)
        {
            Console.Title = "TranslatorXML";

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
                channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "XML");

                Console.WriteLine(" TranslatorXML ready ");

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

                    // Creating XML request
                    XElement bankRequestXML =
                        new XElement("LoanRequest",
                            new XElement("ssn", ssn.Replace("-", "")),
                            new XElement("creditScore", creditScore),
                            new XElement("loanAmount", amount),
                            new XElement("loanDuration", convertDate(duration))
                      );

                    string println = "Loan request sent to " + bank.Bankname;
                    string replyAddress = bank.Normalizer;

                    BankEngine be = new BankEngine();
                    be.sendMessageReplyAddress(bank.Exchange, bankRequestXML.ToString(), println, replyAddress);

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static string convertDate(int duration)
        {

            int years = 0;
            int months = 0;

            while (true)
            {

                int tal = duration - 12;
                duration = duration - 12;

                if (tal < 0)
                {
                    months = tal + 12;
                    break;
                }

                years++;

            }

            return 1970 + years + "-" + 1 + months + "-01 01:00:00.0 CET";
        }




    }
}
