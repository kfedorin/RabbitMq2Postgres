using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Handlers.OrganizationQuery
{
    // Query
    public class GetAllOrganizationQuery : IRequest<IEnumerable<Organization>> { }

    // Query Handler
    public class GetOrganizationQueryHandler : IRequestHandler<GetAllOrganizationQuery, IEnumerable<Organization>>
    {
        private readonly IDbContext _dbContext;

        public GetOrganizationQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Organization>> Handle(GetAllOrganizationQuery query, CancellationToken cancellationToken)
        {
            var entityList = _dbContext.Set<Organization>().AsNoTracking();

            var organizations = await entityList.ToListAsync(cancellationToken: cancellationToken);

            return organizations;
        }
    }
}
