using System.ComponentModel.DataAnnotations;
using AutoMapper;
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


            var entity = mapper.Map<User>(request);

            await _dbContext.Set<User>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken: cancellationToken);

            var updatedEntity = _dbContext.Set<User>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<User>(updatedEntity.Entity);
        }
    }
}
