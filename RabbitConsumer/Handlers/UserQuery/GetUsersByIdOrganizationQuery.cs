using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Handlers.UserQuery
{
    public class GetUsersByIdOrganizationQuery : IRequest<IEnumerable<User>>
    {
        public int IdOrganization { get; set; }
        public int ItemsOnPage { get; set; }
        public int Page { get; set; }

        public GetUsersByIdOrganizationQuery(int idOrganization, int itemsOnPage, int page)
        {
            IdOrganization = idOrganization;
            ItemsOnPage = itemsOnPage;
            Page = page;
        }
    }


    // Query Handler
    public class GetUsersByIdOrganizationQueryHandler : IRequestHandler<GetUsersByIdOrganizationQuery, IEnumerable<User>>
    {
        private readonly IDbContext _dbContext;

        public GetUsersByIdOrganizationQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersByIdOrganizationQuery query, CancellationToken cancellationToken)
        {
            var result = _dbContext.Set<User>().AsNoTracking().Where(c => c.IdOrganization == query.IdOrganization);

            if (query.ItemsOnPage <= 0 || query.Page <= 0) return result;
            return result.Skip((query.Page - 1) * query.ItemsOnPage).Take(query.ItemsOnPage).ToList();
        }
    }
}
