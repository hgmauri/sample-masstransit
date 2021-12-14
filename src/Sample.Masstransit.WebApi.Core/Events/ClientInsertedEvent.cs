
namespace Sample.Masstransit.WebApi.Core.Events;

public class ClientInsertedEvent
{
    public string? ClientId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
}