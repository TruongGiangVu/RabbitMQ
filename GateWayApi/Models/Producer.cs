using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;

namespace GateWayApi.Models
{
    public class Producer
    {
        public ConnectionFactory _factory {get; set;}
        public IConnection _connection {get; set;}
        public IModel _channel {get; set;}
        public string queueName {get; set;}
        public string exchangeName {get; set;}

        public Producer()
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
        public void Publish(string message = "Hello world"){
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "exchange_demo",
                                routingKey: this.queueName,
                                basicProperties: null,
                                body: body);
        }
    }
}