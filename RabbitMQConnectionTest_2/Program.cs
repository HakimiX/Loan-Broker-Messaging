using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConnectionTest_2
{
    class Program
    {
        private const string QUEUE_NAME = "JyskeBank3_Queue";
        private const string EXCHANGE_NAME = "JyskeBank3_exchange";

        static void Main(string[] args)
        {
            Console.Title = "SENDER";

            while(true){
                Send("Mavi");
            }
        }
        public static void Send(string message)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                //Port = 5672,
                UserName = "student",
                Password = "cph"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                 

                var body = Encoding.UTF8.GetBytes(message);

                channel.QueueDeclare(queue: QUEUE_NAME,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                
                // Declare Exchange
                channel.ExchangeDeclare("JyskeBank3_exchange", ExchangeType.Topic);

                // Bind Queue
                channel.QueueBind("JyskeBank3_Queue", "JyskeBank3_exchange", "");

                channel.BasicPublish(exchange: EXCHANGE_NAME,
                                                   routingKey: "",
                                                   basicProperties: null,
                                                   body: body);


                Console.WriteLine(" [x] Sent {0}", message);

            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }



    }
}
