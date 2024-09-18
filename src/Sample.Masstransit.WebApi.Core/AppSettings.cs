
namespace Sample.Masstransit.WebApi.Core;
public class AppSettings
{
    public DistributedTracingOptions DistributedTracing { get; set; }
}

public class DistributedTracingOptions
{
    public JaegerOptions Jaeger { get; set; }
}

public class JaegerOptions
{
    public string ServiceName { get; set; }
}