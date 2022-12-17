using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Controllers.Base;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace GPNA.ObjectModel.Controllers
{
    [Route("api/[controller]")]
    public class BranchController : TechnologyControllerBase<Organization>
    {
        public BranchController(ITechnology<Organization> repos) : base(repos)
        {
        }

    }
}
