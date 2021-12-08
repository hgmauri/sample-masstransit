using MassTransit;
using MassTransit.Definition;
using Sample.Masstransit.WebApi.Core.Events;
using Sample.Masstransit.Worker.Workers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, collection) =>
    {
        collection.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumer<TimerVideoConsumer>(typeof(TimerVideoConsumerDefinition));
            x.AddConsumer<QueueClientConsumer>(typeof(QueueClientConsumerDefinition));
            x.AddRequestClient<ConvertVideoEvent>();

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));
                cfg.UseDelayedMessageScheduler();
                //cfg.ConnectReceiveObserver(new ReceiveObserverExtensions());
                cfg.ServiceInstance(instance =>
                {
                    instance.ConfigureJobServiceEndpoints();
                    instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                });
            });
        });
        collection.AddMassTransitHostedService(true);
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Waiting for new messages.");

while (true) ;