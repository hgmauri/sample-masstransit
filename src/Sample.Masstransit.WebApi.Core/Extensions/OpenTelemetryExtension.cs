using Microsoft.Extensions.DependencyInjection;
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
				.AddService(appSettings?.DistributedTracing?.Jaeger?.ServiceName ?? "Service");

			telemetry.AddSource("MassTransit")
				.AddMassTransitInstrumentation()
				.SetResourceBuilder(resourceBuilder)
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.SetSampler(new AlwaysOnSampler());

			telemetry.AddOtlpExporter();
		});
	}
}