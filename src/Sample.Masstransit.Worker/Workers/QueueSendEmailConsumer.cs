using MassTransit;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class QueueSendEmailConsumer : IConsumer<SendEmailEvent>
{
    public Task Consume(ConsumeContext<SendEmailEvent> context)
    {
	    Serilog.Log.Information($"Email enviado com sucesso: {context.Message.Email}");
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