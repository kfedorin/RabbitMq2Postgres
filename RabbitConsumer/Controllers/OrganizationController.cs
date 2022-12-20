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

        /// <summary>
        /// Получить все организации
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrganizationQuery());
            return Ok(result);
        }

        /// <summary>
        /// Получить организацию по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="400">Ошибка запроса</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetOrganizationByIdQuery(id));

            if (result is null)
                return BadRequest(id);

            return Ok(result);
        }

        /// <summary>
        /// Добавить организацию
        /// </summary>
        /// <param name="newOrganization"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrganizationCommand newOrganization)
        {
            return Ok(await _mediator.Send(newOrganization));
        }

        /// <summary>
        /// Обновить орагинзацию
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrganizationCommand organization)
        {
            return Ok(await _mediator.Send(organization));
        }

        /// <summary>
        /// Удалить организацию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="400">Ошибка запроса</response>
        /// <response code="204">Организация успешна удалена</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _mediator.Send(new DeleteOrganizationCommand() { Id = id });

            if (result) return NoContent();
            return BadRequest(id);
        }
    }
}
