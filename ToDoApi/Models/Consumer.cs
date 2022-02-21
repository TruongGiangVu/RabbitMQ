using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;

namespace ToDoApi.Models
{
    public class Consumer
    {
        public ConnectionFactory _factory {get; set;}
        public IConnection _connection {get; set;}
        public IModel _channel {get; set;}
        public string queueName {get; set;}
        public string exchangeName {get; set;}

        public Consumer()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection(); 
            _channel = _connection.CreateModel();
        }

        public void InitQueue(string name = "hello")
        {
            this.queueName = name;
            _channel.QueueDeclare(
                queue: name, 
                durable: false, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null);
        }
        public string Receive(string message = "Hello world"){
            string response = "";
            var consumer = new EventingBasicConsumer(_channel);
                // Received event'i sürekli listen modunda olacaktır.
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                response = message;
            };

            _channel.BasicConsume(queueName, true, consumer);
            return response;
        }
    }
}