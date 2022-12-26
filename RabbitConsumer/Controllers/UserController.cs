using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Commands.OrganizationCommand;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Handlers.UserQuery;

namespace RabbitConsumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateUserCommand> _validatorCreate;
        private readonly IValidator<UpdateUserCommand> _validatorUpdate ;

        public UserController(IMediator mediator, IValidator<CreateUserCommand> validatorCreate, IValidator<UpdateUserCommand> validatorUpdate)
        {
            _mediator = mediator;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
        }
  

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserQuery());
            return Ok(result);
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet("GetByIdUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (result is null)
                return NotFound(id);

            return Ok(result);
        }

        /// <summary>
        /// Получить пользователей по ID организации
        /// </summary>
        /// <param name="idOrganization"></param>
        /// <returns></returns>
        /// <response code="404">Организация не найдена</response>
        [HttpGet("GetByIdOrganization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdOrganization(int idOrganization,
            [Range(0, 10000)] int itemsOnPage = 10,
            [Range(1, 100000)] int page = 1)
        {
            var result = await _mediator.Send(new GetUsersByIdOrganizationQuery(idOrganization, itemsOnPage, page));

            if (result is null)
                return NotFound(idOrganization);

            return Ok(result);
        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateUserCommand newUser)
        {
            //if (!ModelState.IsValid)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            //}

            var validation = _validatorCreate.Validate(newUser);

            if (!validation.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validation);
            }

            return Ok(await _mediator.Send(newUser));
        }

        /// <summary>
        /// Добавить случайного пользователя
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("SeedPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(SeedUserCommand newUser)
        {
            return Ok(await _mediator.Send(newUser));
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="400">Ошибка запроса</response>
        [HttpPut]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand user)
        {
            //var validationResult = await validator.ValidateAsync(product);
            var validation = await _validatorUpdate.ValidateAsync(user);

            if (!validation.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validation);
            }

            return Ok(await _mediator.Send(user));
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="204">Пользователь успешно удален</response>
        [HttpDelete("DeleteByIdUser")]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand() { Id = id });

            if (result) return NoContent();
            return NotFound(id);
        }
    }
}
