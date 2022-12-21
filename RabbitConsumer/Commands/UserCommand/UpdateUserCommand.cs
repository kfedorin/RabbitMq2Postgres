using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.UserCommand
{
    public class UpdateUserCommand : IRequest<User>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int IdOrganization { get; set; }

    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly IDbContext _dbContext;
        public UpdateUserCommandHandler(IDbContext dbContext) : base()
        {
            _dbContext = dbContext;
        }


        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateUserCommand, User>();
            }).CreateMapper();

            var entityToUpdate = await _dbContext.Set<User>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken: cancellationToken);

            entityToUpdate.IdOrganization = request.IdOrganization;

            var entity = mapper.Map<User>(entityToUpdate);

            var updatedEntity = _dbContext.Set<User>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<User>(updatedEntity.Entity);
        }

        public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
        {
            public UpdateUserCommandValidator(IDbContext dbContext)
            {
                RuleFor(e => e.Id)
                    .MustAsync(async (id, cancellation) =>
                    {
                        return await dbContext.Set<User>()
                            .AnyAsync(e => e.Id == id, cancellation);
                    }).WithMessage("Пользователя с данным Id не существует");

                RuleFor(e => e.IdOrganization)
                    .MustAsync(async (idOrganization, cancellation) =>
                    {
                        return await dbContext.Set<Organization>()
                            .AnyAsync(e => e.Id == idOrganization, cancellation);
                    }).WithMessage("Организации с таким Id не существует");
            }
        }
    }
}
