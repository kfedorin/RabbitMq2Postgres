using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.OrganizationCommand
{
    public class SeedOrganizationCommand : IRequest<Organization>
    {
    

    }

    public class SeedOrganizationCommandHandler : IRequestHandler<SeedOrganizationCommand, Organization>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        public SeedOrganizationCommandHandler(IDbContext dbContext, IMapper mapper) : base()
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<Organization> Handle(SeedOrganizationCommand request, CancellationToken cancellationToken)
        {
            Random random = new Random();

            var organization = new Organization
            {
                Name = "Nane_" + random.Next(1, 100).ToString()
            };

            var createdEntity = await _dbContext.Set<Organization>().AddAsync(organization, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Organization>(createdEntity.Entity); ;
        }
    }
}
