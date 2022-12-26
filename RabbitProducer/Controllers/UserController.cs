using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitModel;
using Serilog;

namespace RabbitProducer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IPublishEndpoint _publishEndpoint;

        public UserController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            await _publishEndpoint.Publish(user, CancellationToken.None);

            var serializedMessage = JsonSerializer.Serialize(user);

            Log.Information($"Send message: {serializedMessage}");

            return Ok();
        }
    }
}