using System.Diagnostics;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.Metadata;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueClientUpdatedConsumer : IConsumer<ClientUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ClientUpdatedEvent> context)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            var id = context.Message.ClientId;
            var name = context.Message.Name;

            Console.WriteLine($"Receive client: {id} - {name}");
            await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<ClientUpdatedEvent>.ShortName);
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<ClientUpdatedEvent>.ShortName, ex);
        }
    }
}

public class QueueClientUpdatedConsumerDefinition : ConsumerDefinition<QueueClientInsertedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueClientInsertedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
    }
}