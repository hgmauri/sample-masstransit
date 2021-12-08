using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Sample.Masstransit.WebApi.Core;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, collection) =>
    {
        collection.AddMassTransit(bus =>
        {
            bus.AddConsumers(Assembly.GetEntryAssembly());

            bus.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));

                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(BusMessages.PublishClientInserted, false));
                cfg.UseMessageRetry(retry =>
                {
                    retry.Interval(3, TimeSpan.FromSeconds(5));
                });
                cfg.PrefetchCount = 10;
            });
        });
        collection.AddMassTransitHostedService(true);
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Waiting for new messages.");

while (true) ;