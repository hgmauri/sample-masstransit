using MassTransit;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.Worker.Workers;

public class TimerVideoConsumer : IJobConsumer<ConvertVideoEvent>
{
    public async Task Run(JobContext<ConvertVideoEvent> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
    }
}

public class TimerVideoConsumerDefinition : ConsumerDefinition<TimerVideoConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TimerVideoConsumer> consumerConfigurator)
    {
        consumerConfigurator.Options<JobOptions<ConvertVideoEvent>>(options =>
            options.SetRetry(r => r.Interval(3, TimeSpan.FromSeconds(30))).SetJobTimeout(TimeSpan.FromMinutes(1)).SetConcurrentJobLimit(10));
    }
}