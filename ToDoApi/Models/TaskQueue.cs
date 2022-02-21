
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

namespace ToDoApi.Models
{
    public class TaskQueue : IHostedService
    {
        Consumer consumer = new Consumer();
        public ToDoContext _context { get; set; }
        public TaskQueue(IServiceScopeFactory factory)
        {
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<ToDoContext>();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            consumer.InitQueue("todo");
            string request = consumer.Receive();
            Message message = JsonConvert.DeserializeObject<Message>(request);
            if(message != null && message.type == "post"){
                if(!_context.ToDos.Any(p => p.Id == message.item.Id)){
                     _context.ToDos.AddAsync(message.item);
                    _context.SaveChangesAsync();
                }
            }
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}