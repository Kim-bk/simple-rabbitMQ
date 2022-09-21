using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();

            using var chanel = connection.CreateModel();
            chanel.QueueDeclare("demo-queue",
                durable: true,
                autoDelete: false,
                exclusive: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            chanel.BasicConsume("demo-queue", true, consumer);
            Console.ReadLine();
        }
    }
}
