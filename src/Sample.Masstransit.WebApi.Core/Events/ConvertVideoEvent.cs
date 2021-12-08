namespace Sample.Masstransit.WebApi.Core.Events;

public interface ConvertVideoEvent
{
    string GroupId { get; }
    int Index { get; }
    int Count { get; }
    string Path { get; }
}