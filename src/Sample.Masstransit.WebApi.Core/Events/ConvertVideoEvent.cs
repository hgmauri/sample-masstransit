namespace Sample.Masstransit.WebApi.Core.Events;

public class ConvertVideoEvent
{
    public string GroupId { get; set; }
    public int Index { get; set; }
	public int Count { get; set; }
	public string Path { get; set; }
}