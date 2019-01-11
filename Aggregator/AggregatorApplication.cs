using ClassEngine;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator
{
    class AggregatorApplication
    {
       
        private static List<Bank> listBanks;
        private static Boolean terminateTimer = false;
        private static Dictionary<string, LoanResponse> loanResponseMap = new Dictionary<string, LoanResponse>();
        private static string currentSsn = "";
        static void Main(string[] args)
        {
            listBanks = new List<Bank>();

            Console.Title = "Aggregator";

            ConnectionFactory factory = new ConnectionFactory()

            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel1 = connection.CreateModel())
            using (var channel2 = connection.CreateModel())
            {

                channel1.QueueDeclare(queue: "aggregatorList_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel1.ExchangeDeclare("aggregatorList_exchange", ExchangeType.Topic);
                channel1.QueueBind("aggregatorList_queue", "aggregatorList_exchange", "");

                channel2.QueueDeclare(queue: "aggregator_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel2.ExchangeDeclare("aggregator_exchange", ExchangeType.Topic);
                channel2.QueueBind("aggregator_queue", "aggregator_exchange", "");

                Console.WriteLine(" Aggregator ready ");

                var consumer1 = new EventingBasicConsumer(channel1);
                consumer1.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    string[] messageArray = message.Split('#');
                    currentSsn = messageArray[0];
                    listBanks.Clear();
                    listBanks = JsonConvert.DeserializeObject<List<Bank>>(messageArray[1]);
                    Console.WriteLine(" [x] Received {0}", listBanks.Count + " banks");
                    loanResponseMap.Add(currentSsn, new LoanResponse(100.0, "!"));
                    StartTimer();
                };

                channel1.BasicConsume(queue: "aggregatorList_queue", autoAck: true, consumer: consumer1);
                var consumer2 = new EventingBasicConsumer(channel2);
                consumer2.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    string[] messageArray = message.Split('#');
                    string ssn = messageArray[0];
                    double interestRate = Convert.ToDouble(messageArray[1]);
                    string correlationId = ea.BasicProperties.CorrelationId;
                    Console.WriteLine(" [x] Received {0}", message);
                    if (currentSsn == ssn)
                    {
                        LoanResponse loanResponse;
                        loanResponseMap.TryGetValue(ssn, out loanResponse);
                        if (loanResponse != null)
                        {
                            if (loanResponse.InterestRate > interestRate)
                            {
                                loanResponse.InterestRate = interestRate;
                                loanResponse.FromBank = listBanks.Find(x => x.Normalizer.Contains(correlationId)).Bankname;
                            }
                            loanResponse.Counter++;
                            loanResponseMap[ssn] = loanResponse;
                            if (loanResponse.Counter == listBanks.Count)
                            {
                                terminateTimer = true;
                                BankEngine be = new BankEngine();
                                string messageToSend = currentSsn + "#" + loanResponse.FromBank + "#" + loanResponse.InterestRate;
                                be.sendMessage("loanResponse_exchange", messageToSend, messageToSend);
                                loanResponseMap.Remove(currentSsn);
                            }
                        }
                    }
                };

                channel2.BasicConsume(queue: "aggregator_queue", autoAck: true, consumer: consumer2);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static async void StartTimer()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            if (terminateTimer == false)
            {
                LoanResponse loanResponse;
                loanResponseMap.TryGetValue(currentSsn, out loanResponse);
                BankEngine be = new BankEngine();
                string messageToSend = (loanResponse.FromBank != "!" ? currentSsn + "#" + loanResponse.FromBank + "#" + loanResponse.InterestRate : "No Banks Could Meet Your Wish");
                be.sendMessage("loanResponse_exchange", messageToSend, messageToSend);
                loanResponseMap.Remove(currentSsn);

            }
        }
    }
}
