
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
    public class TaskQueue : IHostedService
    {
        public ToDoContext _context { get; set; }
        private readonly ISubscriber subscriber;
        private readonly IConfiguration _config;
        public TaskQueue(ISubscriber subscriber,IServiceScopeFactory factory, IConfiguration config)
        {
            this.subscriber = subscriber;
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<ToDoContext>();
            _config = config;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            subscriber.Subscribe(ProcessMessage);
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {
            //Console.WriteLine("Hello queue " +_config.GetValue<string>("AppSettings:Key"));
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