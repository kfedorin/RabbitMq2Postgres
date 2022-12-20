using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Handlers.UserQuery;

namespace RabbitConsumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
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
        /// <response code="400">Ошибка запроса</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (result is null)
                return BadRequest(id);

            return Ok(result);
        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand newUser)
        {
            return Ok(await _mediator.Send(newUser));
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserCommand user)
        {
            return Ok(await _mediator.Send(user));
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="400">Ошибка запроса</response>
        /// <response code="204">Пользователь успешно удален</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand() { Id = id });

            if (result) return NoContent();
            return BadRequest(id);
        }
    }
}
