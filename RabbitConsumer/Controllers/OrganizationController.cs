using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Commands;
using RabbitConsumer.Queries;

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
            var product = await _mediator.Send(new CreateOrganizationCommand(newOrganization.Name));
            return product != null ? Created($"/organization/{product.Id}", product) : BadRequest();
        }
    }
}
