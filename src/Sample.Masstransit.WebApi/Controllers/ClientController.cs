using MassTransit;
using Sample.Masstransit.WebApi.Core.Models;

namespace Sample.Masstransit.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<ClientController> _logger;

    public ClientController(ILogger<ClientController> logger, IPublishEndpoint publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ClientModel model)
    {
        await _publisher.Publish<ClientModel>(model);
        _logger.LogInformation($"Send client: {model.ClientId} - {model.Name}");

        return Ok();
    }
}