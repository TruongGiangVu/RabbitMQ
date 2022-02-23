using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;

namespace ToDoApi.Models
{
    public class RabbitBus{
        //private IServiceProvider _sp;
        public ConnectionFactory _factory {get; set;}
        public IConnection _connection {get; set;}
        public IModel _channel {get; set;}
        public string queueName {get; set;}
        public string exchangeName {get; set;}

        public RabbitBus()
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
        public void ProducerQueue(string message = "Hello world"){
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                routingKey: this.queueName,
                                basicProperties: null,
                                body: body);
        }
        public void BasicQos(){
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
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
        public void QueueBind(string queueName = "",string routeKey =""){
            if(queueName.Trim() == "")
                queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routeKey);
        }
        public string InitConsumer()
        {
            string message = "";
            var consumer = new EventingBasicConsumer(_channel);

            // handle the Received event on the consumer
            // this is triggered whenever a new message
            // is added to the queue by the producer
            consumer.Received += (model, ea) =>
            {
                // read the message bytes
                var body = ea.Body.ToArray();
                
                // convert back to the original string
                // {index}|SuperHero{10000+index}|Fly,Eat,Sleep,Manga|1|{DateTime.UtcNow.ToLongDateString()}|0|0
                // is received here
                message = Encoding.UTF8.GetString(body);
                
                //Console.WriteLine(" [x] Received {0}", message);
            };
            _channel.BasicConsume(queue: this.queueName, 
                                autoAck: true,
                                consumer: consumer);
            return message;
        }
    }
}