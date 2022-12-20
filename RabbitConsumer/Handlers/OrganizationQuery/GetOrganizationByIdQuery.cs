using MediatR;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Handlers.OrganizationQuery
{
    public class GetOrganizationByIdQuery : IRequest<Organization>
    {
        public int Id { get; set; }

        public GetOrganizationByIdQuery(int id)
        {
            Id = id;
        }
    }


    // Query Handler
    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Organization>
    {
        private readonly IDbContext _dbContext;

        public GetOrganizationByIdQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Organization> Handle(GetOrganizationByIdQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Organization>().FirstOrDefault(c => c.Id == query.Id);

        }
    }
}
