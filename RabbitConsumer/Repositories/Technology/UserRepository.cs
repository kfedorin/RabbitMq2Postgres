using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Repositories.Base;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Repositories.Technology
{
    public class UserRepository : TechnologyRepositoryBase<User>
    {
        private readonly RabbitTestContext _db;

        public UserRepository(RabbitTestContext db) : base(db)
        {
            _db = db;
        }

        protected override DbSet<User> DbSet => _db.Users;

    }
}
