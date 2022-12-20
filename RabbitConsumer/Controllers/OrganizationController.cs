using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Commands.OrganizationCommand;
using RabbitConsumer.Handlers.OrganizationQuery;

namespace RabbitConsumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizationController(IMediator mediator) => _mediator = mediator;

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrganizationQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetOrganizationByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrganizationCommand newOrganization)
        {
            return Ok(await _mediator.Send(newOrganization));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrganizationCommand organization)
        {
            return Ok(await _mediator.Send(organization));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            await _mediator.Send(new DeleteOrganizationCommand() { Id = id });
            return NoContent();
        }
    }
}
