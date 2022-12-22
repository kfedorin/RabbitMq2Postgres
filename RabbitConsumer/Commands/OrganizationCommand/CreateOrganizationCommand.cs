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

    }

    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Organization>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        public CreateOrganizationCommandHandler(IDbContext dbContext, IMapper mapper) : base()
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<Organization> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<CreateOrganizationCommand, Organization>();
            //}).CreateMapper();


            var entity = _mapper.Map<Organization>(request);

            var createdEntity = await _dbContext.Set<Organization>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Organization>(createdEntity.Entity);
        }
    }



}
