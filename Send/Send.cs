using System;
using RabbitMQ.Client;
using System.Text;
 
namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            // create connection to localhost
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(IConnection connection = factory.CreateConnection())
            using(IModel channel = connection.CreateModel()) // create channel
            {
                // create queue name 'hello'
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                // message
                var random = new Random();

                string message = "Hello World!"+ random.Next(1, 1000000);
                byte[] body = Encoding.UTF8.GetBytes(message);
                // send message
                channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            }
    }
}
