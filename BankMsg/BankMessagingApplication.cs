using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace BankMsg
{
    class BankMESSAGINGApplication
    {
        private const string QUEUE_NAME = "LunarWay_queue";
        private const string EXCHANGE_NAME = "LunarWay_exchange";

        static void Main(string[] args)
        {
            Console.Title = "BankMESSAGING";

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

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout);
                channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var message = Encoding.UTF8.GetString(body);
                    string[] messageArray = message.Split('#');
                    string ssn = messageArray[0];
                    int creditScore = Convert.ToInt32(messageArray[1]);
                    double loanAmount = Convert.ToDouble(messageArray[2]);
                    int loanDuration = Convert.ToInt32(messageArray[3]);

                    double interestRate = getInterestRate(loanAmount, loanDuration, creditScore);
                    string messageToSend = ssn + "#" + interestRate;

                    SendResponseMessage(props, messageToSend);

                };
                channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void SendResponseMessage(IBasicProperties props, string message)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                 routingKey: props.ReplyTo,
                                 basicProperties: props,
                                 body: body);

            }
        }

        private static double getInterestRate(double loanAmount, int loanDuration, int creditScore)
        {
            double baseRate = 2.5;

            double interestRate = baseRate + ((800 - creditScore) / 100);

            if (loanAmount < 10000)
            {
                interestRate += 0.3;
            }
            else if (loanAmount > 70000)
            {
                interestRate += 0.25;
            }
            else
            {
                interestRate += 0.1;
            }

            if (loanDuration > 12)
            {
                interestRate += 0.2;
            }
            else if (loanDuration < 40)
            {
                interestRate += 0.5;
            }
            else
            {
                interestRate += 0.3;
            }

            return interestRate;
        }
    }
}
