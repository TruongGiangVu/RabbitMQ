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
        public void BasicQos(){
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
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
        public void ProducerQueue(string message = "Hello world"){
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                routingKey: this.queueName,
                                basicProperties: null,
                                body: body);
        }
        public void InitExChange(string name = "hello"){
            exchangeName = name;
            _channel.ExchangeDeclare(name, ExchangeType.Topic);
        }
        public void ProducerExchange(string routeKey ="", string message ="hello world"){
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: exchangeName,
                                routingKey: routeKey,
                                basicProperties: null,
                                body: body);
        }
    }
}