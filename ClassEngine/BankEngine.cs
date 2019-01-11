using System;
using RabbitMQ.Client;
using System.Text;

namespace ClassEngine
{
    public class BankEngine
    {
        public BankEngine()
        {
            
        }
       
        public void sendMessage(string EXCHANGE_NAME, string message, string println)
        {
            // Set Connection Info
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            // Create Connection and Channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) 
            {
                // Publish Message
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);
                channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: "", basicProperties: null, body: body);

                Console.WriteLine(" [x] Sent {0}", println);
            }

            Console.WriteLine(" Press [enter] to exit ");
        }

        public void sendMessage(string EXCHANGE_NAME, string message, string println, string routingkey)
        {
            // Set Connection Info
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            // Create Connection and Channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Publish Message
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);
                channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: routingkey, basicProperties: null, body: body);

                Console.WriteLine(" [x] Sent {0}", println);
            }

            Console.WriteLine(" Press [enter] to exit ");
        }


        public void sendMessageReplyAddress(string EXCHANGE_NAME, string message, string println, string replyAddress)
        {
            // Set Connection Info
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            // Create Connection and Channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Publish Message
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.ReplyTo = replyAddress;
                channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: "", basicProperties: properties, body: body);

                Console.WriteLine(" [x] Sent {0}", println);
            }

            Console.WriteLine(" Press [enter] to exit ");
        }

        public void sendMessageCorrelationId(string EXCHANGE_NAME, string message, string println, string correlationId)
        {
            // Set Connection Info
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph"
            };

            // Create Connection and Channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) 
            {
                // Publish Message
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.CorrelationId = correlationId;
                channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: "", basicProperties: properties, body: body);

                Console.WriteLine(" [x] Sent {0}", println);
            }

            Console.WriteLine(" Press [enter] to exit ");
        }
    }
}
