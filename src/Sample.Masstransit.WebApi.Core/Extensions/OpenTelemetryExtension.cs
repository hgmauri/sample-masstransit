using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Sample.Masstransit.WebApi.Core.Extensions;

public static class OpenTelemetryExtension
{
	public static void AddOpenTelemetry(this IServiceCollection services, AppSettings appSettings)
	{
		services.AddOpenTelemetry().WithTracing(telemetry =>
		{
			var resourceBuilder = ResourceBuilder.CreateDefault()
				.AddTelemetrySdk()
				.AddEnvironmentVariableDetector()
				.AddService(appSettings?.DistributedTracing?.Jaeger?.ServiceName);

			telemetry.AddSource("MassTransit")
				.AddMassTransitInstrumentation()
				.SetResourceBuilder(resourceBuilder)
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.SetSampler(new AlwaysOnSampler())
				.AddJaegerExporter(jaegerOptions =>
				{
					jaegerOptions.AgentHost = appSettings?.DistributedTracing?.Jaeger?.Host;
					jaegerOptions.AgentPort = appSettings?.DistributedTracing?.Jaeger?.Port ?? 0;
				});
		});
	}
}