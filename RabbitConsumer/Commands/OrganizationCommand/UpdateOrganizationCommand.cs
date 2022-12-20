using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.OrganizationCommand
{
    public class UpdateOrganizationCommand : IRequest<Organization>
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, Organization>
    {
        private readonly IDbContext _dbContext;
        public UpdateOrganizationCommandHandler(IDbContext dbContext) : base()
        {
            _dbContext = dbContext;
        }


        public async Task<Organization> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateOrganizationCommand, Organization>();
            }).CreateMapper();


            var entity = mapper.Map<Organization>(request);

            var entityToUpdate = await _dbContext.Set<Organization>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken: cancellationToken);

            var updatedEntity = _dbContext.Set<Organization>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<Organization>(updatedEntity.Entity);
        }
    }
}
