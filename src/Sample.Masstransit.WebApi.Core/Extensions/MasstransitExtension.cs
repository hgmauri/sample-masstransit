using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Masstransit.WebApi.Core.Extensions;

public static class MasstransitExtension
{
    public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(bus =>
        {
            bus.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq"));
                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(BusMessages.PublishClientInserted, false));
                cfg.UseMessageRetry(retry =>
                {
                    retry.Interval(3, TimeSpan.FromSeconds(5));
                });
            });
        });
        services.AddMassTransitHostedService();
    }
}