using MassTransit;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueSendEmailConsumer : IConsumer<SendEmailEvent>
{
    private readonly ILogger<QueueSendEmailConsumer> _logger;

    public QueueSendEmailConsumer(ILogger<QueueSendEmailConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SendEmailEvent> context)
    {
	    _logger.LogInformation($"Email successfully sent: {context.Message.Email}");
	    return Task.CompletedTask;
    }
}

public class QueueSendEmailConsumerDefinition : ConsumerDefinition<QueueSendEmailConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueSendEmailConsumer> consumerConfigurator, IRegistrationContext context)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}