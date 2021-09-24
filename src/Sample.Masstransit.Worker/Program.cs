using System;
using System.Threading;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sample.Masstransit.WebApi.Core.Extensions;
using Sample.Masstransit.Worker.Workers;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(configuration);
    })
    .ConfigureServices((context, collection) =>
    {
        collection.AddMassTransitExtension(context.Configuration);
    })
    .Build();

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("queue-teste", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<WorkerClient>();
    });
});
var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

Console.WriteLine("Waiting for new messages.");

while (true);
