using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Controllers.Base;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationController : TechnologyControllerBase<Organization>
    {
        public OrganizationController(ITechnology<Organization> repos) : base(repos)
        {
        }

    }
}
