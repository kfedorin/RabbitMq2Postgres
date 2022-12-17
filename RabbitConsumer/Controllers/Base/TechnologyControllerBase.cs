using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Base;

namespace RabbitConsumer.Controllers.Base
{
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiController]
        public abstract class TechnologyControllerBase<T> : ControllerBase
            where T : EntityBase
    {
        protected readonly ITechnology<T> Repos;
        public TechnologyControllerBase(ITechnology<T> repos)
        {
            Repos = repos;
        }

        /// <summary>
        /// Получить все
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<T>>> GetAll()
        {
            var item = await Repos.GetAll();
            return item.ToList();
        }

        /// <summary>
        /// Получить по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <response code="404">Объект отсутствует, или удален</response>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> GetOne(
            [Required]
            [Range(1, int.MaxValue)] int id)
        {
            var item = await Repos.GetOne(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<T>> Post([FromBody] T item)
        {
            await Repos.Post(item);
            return CreatedAtAction(nameof(GetOne), new { item.Id }, item);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<T>> Put(int id, [FromBody] T item)
        {
            await Repos.Put(id, item);
            return CreatedAtAction(nameof(GetOne), new { item.Id }, item);
        }
     
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await Repos.Delete(id);
        }
    }
}
