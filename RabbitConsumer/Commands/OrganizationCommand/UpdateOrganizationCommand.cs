using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.OrganizationCommand
{
    public class UpdateOrganizationCommand : IRequest<Organization>
    {
        [Required]
        public int Id { get; set; }

        [Required]
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


            var entityToUpdate = await _dbContext.Set<Organization>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken: cancellationToken);

            entityToUpdate.Name = request.Name;

            var entity = mapper.Map<Organization>(entityToUpdate);

            var updatedEntity = _dbContext.Set<Organization>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<Organization>(updatedEntity.Entity);
        }

        public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
        {
            public UpdateOrganizationCommandValidator(IDbContext dbContext)
            {
                RuleFor(e => e.Id)
                    .MustAsync(async (idOrganization, cancellation) =>
                    {
                        return await dbContext.Set<Organization>()
                            .AnyAsync(e => e.Id == idOrganization, cancellation);
                    }).WithMessage("Организации с таким Id не существует");
            }
        }
    }
}
