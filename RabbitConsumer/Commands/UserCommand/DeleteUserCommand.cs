using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.UserCommand
{
    public class DeleteUserCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IDbContext _dbContext;
        public DeleteUserCommandHandler(IDbContext dbContext) : base()
        {
            _dbContext = dbContext;
        }


        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeleteUserCommand, User>();
            }).CreateMapper();

            var entity = mapper.Map<User>(request);

            _dbContext.Set<User>().Remove(entity);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return false;
            }
            
            return true;
        }
    }
}
