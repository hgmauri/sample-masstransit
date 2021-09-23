using System;
using System.Threading;
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
        e.Consumer<WorkerClient>();
        e.PrefetchCount = 10;
    });
});
var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

Console.WriteLine("Waiting for messages...");

while (true);
