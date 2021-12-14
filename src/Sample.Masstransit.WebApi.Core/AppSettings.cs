
namespace Sample.Masstransit.WebApi.Core;
public class AppSettings
{
    public DistributedTracingOptions? DistributedTracing { get; set; }
}

public class DistributedTracingOptions
{
    public bool IsEnabled { get; set; }
    public JaegerOptions? Jaeger { get; set; }
}

public class JaegerOptions
{
    public string? ServiceName { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
}