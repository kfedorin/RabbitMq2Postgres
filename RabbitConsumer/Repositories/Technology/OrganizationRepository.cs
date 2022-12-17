using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Repositories.Base;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Repositories.Technology
{
    public class OrganizationRepository : TechnologyRepositoryBase<Organization>
    {
        private readonly RabbitTestContext _db;

        public OrganizationRepository(RabbitTestContext db) : base(db)
        {
            _db = db;
        }

        protected override DbSet<Organization> DbSet => _db.Organizations;

    }
}
