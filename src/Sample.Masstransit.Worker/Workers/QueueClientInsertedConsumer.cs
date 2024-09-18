using System.Diagnostics;
using MassTransit;
using MassTransit.Metadata;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueClientInsertedConsumer : IConsumer<ClientInsertedEvent>
{
    public async Task Consume(ConsumeContext<ClientInsertedEvent> context)
    {
        var timer = Stopwatch.StartNew();
        var id = context.Message?.ClientId;
        var name = context.Message?.Name;
        var email = context.Message?.Email;

		try
        {
	        Serilog.Log.Information($"Recebendo evento: {nameof(ClientInsertedEvent)}: {id} - {name}");

			await context.Publish(new SendEmailEvent { Email = email });

            await context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<ClientInsertedEvent>.ShortName);
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<ClientInsertedEvent>.ShortName, ex);
        }
        Serilog.Log.Information($"Evento concluido! {nameof(ClientInsertedEvent)}: {id} - {name}");
    }
}

public class QueueClientConsumerDefinition : ConsumerDefinition<QueueClientInsertedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueClientInsertedConsumer> consumerConfigurator, IRegistrationContext context)
	{
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}