using MassTransit;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueClientUpdatedConsumer : IConsumer<ClientUpdatedEvent>
{
    public Task Consume(ConsumeContext<ClientUpdatedEvent> context)
    {
        if (context.Message.Name == "test")
            throw new ArgumentException("Invalid");

        var id = context.Message?.ClientId;
        var name = context.Message?.Name;

        Serilog.Log.Information($"Evento concluído: {nameof(ClientUpdatedEvent)}: {id} - {name}");
        return Task.CompletedTask;
    }
}

public class QueueClientUpdatedConsumerDefinition : ConsumerDefinition<QueueClientUpdatedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueClientUpdatedConsumer> consumerConfigurator, IRegistrationContext context)
	{
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}