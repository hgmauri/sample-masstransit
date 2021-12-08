using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Sample.Masstransit.WebApi.Core.Models;

namespace Sample.Masstransit.Worker.Workers;

public class QueueClientConsumer : IConsumer<ClientInsertedEvent>
{
    public Task Consume(ConsumeContext<ClientInsertedEvent> context)
    {
        var id = context.Message.ClientId;
        var name = context.Message.Name;

        Console.WriteLine($"Receive client: {id} - {name}");
        return Task.CompletedTask;
    }
}

public class QueueClientConsumerDefinition : ConsumerDefinition<QueueClientConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueClientConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
    }
}