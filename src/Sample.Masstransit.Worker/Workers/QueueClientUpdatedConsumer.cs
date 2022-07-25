using MassTransit;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueClientUpdatedConsumer : IConsumer<ClientUpdatedEvent>
{
    private readonly ILogger<QueueClientUpdatedConsumer> _logger;

    public QueueClientUpdatedConsumer(ILogger<QueueClientUpdatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClientUpdatedEvent> context)
    {
        if (context.Message.Name == "test")
        {
            throw new ArgumentException("Invalid");
        }

        var id = context.Message.ClientId;
        var name = context.Message.Name;

        _logger.LogInformation($"Receive client: {id} - {name}");
    }
}

public class QueueClientUpdatedConsumerDefinition : ConsumerDefinition<QueueClientUpdatedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueClientUpdatedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}