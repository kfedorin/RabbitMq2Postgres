using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Controllers.Base;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : TechnologyControllerBase<User>
    {
        public UserController(ITechnology<User> repos) : base(repos)
        {
        }

    }
}
