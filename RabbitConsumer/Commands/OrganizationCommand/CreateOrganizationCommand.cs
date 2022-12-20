using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.OrganizationCommand
{
    public class CreateOrganizationCommand : IRequest<Organization>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int IdOrganization { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleNane { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        public CreateOrganizationCommand(string name)
        {
            Name = name;
        }
    }

    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Organization>
    {
        private readonly IDbContext _dbContext;
        public CreateOrganizationCommandHandler(IDbContext dbContext) : base()
        {
            _dbContext = dbContext;
        }


        public async Task<Organization> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrganizationCommand, Organization>();
            }).CreateMapper();


            var entity = mapper.Map<Organization>(request);

            var createdEntity = await _dbContext.Set<Organization>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<Organization>(createdEntity.Entity);
        }
    }



}
