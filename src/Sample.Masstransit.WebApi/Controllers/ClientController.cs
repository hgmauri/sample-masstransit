using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly IMessageScheduler _publisherScheduler;

    public ClientController(IPublishEndpoint publisher, IMessageScheduler publisherScheduler)
    {
        _publisher = publisher;
        _publisherScheduler = publisherScheduler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ClientInsertedEvent insertedEvent)
    {
        await _publisher.Publish(insertedEvent);
        Serilog.Log.Information($"Evento enviado: {nameof(ClientInsertedEvent)} - {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }

    [HttpPost("update")]
    public async Task<IActionResult> PostUpdate([FromBody] ClientUpdatedEvent insertedEvent)
    {
        await _publisher.Publish(insertedEvent);
        Serilog.Log.Information($"Evento enviado: {nameof(ClientUpdatedEvent)} - {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> PostSchedule([FromBody] ClientInsertedEvent insertedEvent)
    {
        await _publisherScheduler.SchedulePublish(DateTime.UtcNow + TimeSpan.FromSeconds(10), insertedEvent);

        Serilog.Log.Information($"Evento enviado: {nameof(ClientInsertedEvent)} - {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }
}