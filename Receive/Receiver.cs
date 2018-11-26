using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    public class Receiver
    {
        public void Start()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;

                    channel.BasicConsume("hello",
                        true,
                        consumer);

                    Console.ReadKey();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
        }
    }
}