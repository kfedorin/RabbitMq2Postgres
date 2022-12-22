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
        private readonly IMapper _mapper;
        public UpdateOrganizationCommandHandler(IDbContext dbContext, IMapper mapper) : base()
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<Organization> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var entityToUpdate = await _dbContext.Set<Organization>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken: cancellationToken);

            entityToUpdate.Name = request.Name;

            var entity = _mapper.Map<Organization>(entityToUpdate);

            var updatedEntity = _dbContext.Set<Organization>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Organization>(updatedEntity.Entity);
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
