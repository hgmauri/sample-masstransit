using MassTransit;
using MassTransit.Contracts.JobService;
using Sample.Masstransit.WebApi.Core.Events;
using Sample.Masstransit.WebApi.Core.Models;

namespace Sample.Masstransit.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly IMessageScheduler _publisherScheduler;
    private readonly IRequestClient<ConvertVideoEvent> _client;
    private readonly ILogger<ClientController> _logger;

    public ClientController(ILogger<ClientController> logger, IPublishEndpoint publisher, 
        IMessageScheduler publisherScheduler, IRequestClient<ConvertVideoEvent> client)
    {
        _logger = logger;
        _publisher = publisher;
        _publisherScheduler = publisherScheduler;
        _client = client;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ClientInsertedEvent insertedEvent)
    {
        await _publisher.Publish(insertedEvent);
        _logger.LogInformation($"Send client: {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> PostSchedule([FromBody] ClientInsertedEvent insertedEvent)
    {
        await _publisherScheduler.SchedulePublish(DateTime.UtcNow + TimeSpan.FromSeconds(20), insertedEvent);

        _logger.LogInformation($"Send client: {insertedEvent.ClientId} - {insertedEvent.Name}");

        return Ok();
    }

    [HttpPost("wait")]
    public async Task<IActionResult> PostWaitResponse([FromBody] ClientInsertedEvent insertedEvent)
    {
        var groupId = NewId.Next().ToString();
        var path = "test";

        var response = await _client.GetResponse<JobSubmissionAccepted>(new
        {
            path,
            groupId,
            Index = 0,
            Count = 1
        });
        _logger.LogInformation($"Response client: {response.Message.JobId}");

        return Ok(response.Message.JobId);
    }
}