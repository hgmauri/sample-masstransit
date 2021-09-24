using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Masstransit.WebApi.Core.Models;

namespace Sample.Masstransit.Worker.Workers
{
    public class WorkerClient : IConsumer<ClientModel>
    {
        public WorkerClient()
        {
        }

        public Task Consume(ConsumeContext<ClientModel> context)
        {
            var id = context.Message.ClientId;
            var name = context.Message.Name;

            Console.WriteLine($"New client: [{id}] - {name}");
            return Task.CompletedTask;
        }
    }
}