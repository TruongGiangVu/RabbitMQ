
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace GateWayApi.Models
{
    public class TaskQueue : IHostedService
    {
        Consumer consumer = new Consumer();
        public Task StartAsync(CancellationToken cancellationToken)
        {
            consumer.InitQueue("todo");
            string a = consumer.Receive();
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