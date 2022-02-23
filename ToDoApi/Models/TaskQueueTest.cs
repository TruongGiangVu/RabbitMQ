
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Plain.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace ToDoApi.Models
{
    public class TaskQueueTest : IHostedService
    {
        public ToDoContext _context { get; set; }
        public Consumer consumer {get; set;}
        private readonly IConfiguration _config;
        public TaskQueueTest(IServiceScopeFactory factory, IConfiguration config)
        {
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<ToDoContext>();
            //string a = factory.CreateScope().ServiceProvider.GetRequiredService<>();
            _config = config;
            consumer = new Consumer();
            consumer.QueueBind("exchange_demo","queue_todo","todo.*");
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            RunConsumer();
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
        public void RunConsumer(){
            var consumer = new EventingBasicConsumer(this.consumer._channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                ProcessMessage(message);
            };
            this.consumer._channel.BasicConsume(queue: "queue_todo", 
                                autoAck: true,
                                consumer: consumer);
        }
        private bool ProcessMessage(string message)
        {
            Console.WriteLine("task queue: " + message.ToString());
            Message myMessage = JsonConvert.DeserializeObject<Message>(message);
            
            if(myMessage != null && myMessage.type == "post"){
                if(!_context.ToDos.Any(p => p.Id == myMessage.item.Id)){
                     _context.ToDos.AddAsync(myMessage.item);
                    _context.SaveChangesAsync();
                }
            }
            if(myMessage != null && myMessage.type == "put"){
                ToDo todo = _context.ToDos.Where(p => p.Id == myMessage.item.Id).FirstOrDefault();
                if (todo != null)
                {
                    _context.Entry(todo).CurrentValues.SetValues(myMessage.item);
                    _context.SaveChangesAsync();
                }     
            }
            if(myMessage != null && myMessage.type == "delete"){
                ToDo todo = _context.ToDos.Where(p => p.Id == myMessage.item.Id).FirstOrDefault();
                if (todo != null)
                {
                    _context.ToDos.Remove(todo);
                    _context.SaveChangesAsync();
                }    
            }
            return true;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}