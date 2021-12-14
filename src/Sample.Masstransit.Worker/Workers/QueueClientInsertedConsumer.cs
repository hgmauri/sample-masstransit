using System.Diagnostics;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.Metadata;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueClientInsertedConsumer : IConsumer<ClientInsertedEvent>
{
    public async Task Consume(ConsumeContext<ClientInsertedEvent> context)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            var id = context.Message.ClientId;
            var name = context.Message.Name;

            Console.WriteLine($"Receive client: {id} - {name}");
            await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<ClientInsertedEvent>.ShortName);
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<ClientInsertedEvent>.ShortName, ex);
        }
    }
}

public class QueueClientConsumerDefinition : ConsumerDefinition<QueueClientInsertedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueClientInsertedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
    }
}