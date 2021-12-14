using MassTransit;
using MassTransit.Definition;
using Sample.Masstransit.WebApi.Core;
using Sample.Masstransit.WebApi.Core.Events;
using Sample.Masstransit.WebApi.Core.Extensions;
using Sample.Masstransit.Worker.Workers;
using Serilog;

SerilogExtensions.AddSerilog("Worker Sample");

var appSettings = new AppSettings();

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(Log.Logger)
    .ConfigureServices((context, collection) =>
    {
        context.Configuration.Bind(appSettings);
        collection.AddOpenTelemetry(appSettings);
        collection.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumer<TimerVideoConsumer>(typeof(TimerVideoConsumerDefinition));
            x.AddConsumer<QueueClientInsertedConsumer>(typeof(QueueClientConsumerDefinition));
            x.AddConsumer<QueueClientUpdatedConsumer>(typeof(QueueClientUpdatedConsumerDefinition));
            x.AddConsumer<QueueSendEmailConsumer>(typeof(QueueSendEmailConsumerDefinition));
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