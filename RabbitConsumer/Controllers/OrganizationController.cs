using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Commands.OrganizationCommand;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Handlers.OrganizationQuery;

namespace RabbitConsumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateOrganizationCommand> _validator;

        public OrganizationController(IMediator mediator, IValidator<UpdateOrganizationCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        /// <summary>
        /// Получить все организации
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrganizationQuery());
            return Ok(result);
        }

        /// <summary>
        /// Получить организацию по ID
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">Организация не найдена</response>
        [HttpGet("ByIdOrganization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get( int id)
        {
            var result = await _mediator.Send(new GetOrganizationByIdQuery(id));

            if (result is null)
                return NotFound(id);

            return Ok(result);
        }

        /// <summary>
        /// Добавить организацию
        /// </summary>
        /// <param name="newOrganization"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateOrganizationCommand newOrganization)
        {
            return Ok(await _mediator.Send(newOrganization));
        }

        /// <summary>
        /// Добавить случайную организацию
        /// </summary>
        /// <param name="newOrganization"></param>
        /// <returns></returns>
        [HttpPost("SeedPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(SeedOrganizationCommand newOrganization)
        {
            return Ok(await _mediator.Send(newOrganization));
        }

        /// <summary>
        /// Обновить организацию
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        /// <response code="404">Организация с таким ID не найдена</response>
        [HttpPut]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateOrganizationCommand organization)
        {
            var validation = await _validator.ValidateAsync(organization);

            if (!validation.IsValid)
            {
                return StatusCode(StatusCodes.Status404NotFound, validation);
            }

            return Ok(await _mediator.Send(organization));
        }

        /// <summary>
        /// Удалить организацию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="404">Организация с таким ID не найдена</response>
        /// <response code="204">Организация успешна удалена</response>
        [HttpDelete("DeleteByIdOrganization")]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteOrganizationCommand() { Id = id });

            if (result) return NoContent();
            return NotFound(id);
        }
    }
}
