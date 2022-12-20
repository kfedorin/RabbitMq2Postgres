using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Handlers.UserQuery
{
    // Query
    public class GetAllUserQuery : IRequest<IEnumerable<User>> { }

    // Query Handler
    public class GetUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<User>>
    {
        private readonly IDbContext _dbContext;

        public GetUserQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUserQuery query, CancellationToken cancellationToken)
        {
            var entityList = _dbContext.Set<User>().AsNoTracking();

            var organizations = await entityList.ToListAsync(cancellationToken: cancellationToken);

            return organizations;
        }
    }
}
