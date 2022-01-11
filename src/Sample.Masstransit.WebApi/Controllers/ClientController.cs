using MassTransit;
using Sample.Masstransit.WebApi.Core.Events;

namespace Sample.Masstransit.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly IMessageScheduler _publisherScheduler;
    private readonly ILogger<ClientController> _logger;

    public ClientController(ILogger<ClientController> logger, IPublishEndpoint publisher, IMessageScheduler publisherScheduler)
    {
        _logger = logger;
        _publisher = publisher;
        _publisherScheduler = publisherScheduler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ClientInsertedEvent insertedEvent)
    {
        await _publisher.Publish(insertedEvent);
        _logger.LogInformation($"Send client inserted: {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }

    [HttpPost("update")]
    public async Task<IActionResult> PostUpdate([FromBody] ClientUpdatedEvent insertedEvent)
    {
        await _publisher.Publish(insertedEvent);
        _logger.LogInformation($"Send client updated: {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> PostSchedule([FromBody] ClientInsertedEvent insertedEvent)
    {
        await _publisherScheduler.SchedulePublish(DateTime.UtcNow + TimeSpan.FromSeconds(10), insertedEvent);

        _logger.LogInformation($"Send client: {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }
}