using MassTransit;
using Sample.Masstransit.WebApi.Core.Models;

namespace Sample.Masstransit.Worker.Workers;

public class WorkerClient : IConsumer<ClientInsertedEvent>
{
    public Task Consume(ConsumeContext<ClientInsertedEvent> context)
    {
        var id = context.Message.ClientId;
        var name = context.Message.Name;

        Console.WriteLine($"Receive client: {id} - {name}");
        return Task.CompletedTask;
    }
}