using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.UserCommand
{
    public class CreateUserCommand : IRequest<User>
    {

        [Required]
        public int IdOrganization { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IDbContext _dbContext;
        public CreateUserCommandHandler(IDbContext dbContext) : base()
        {
            _dbContext = dbContext;
        }


        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUserCommand, User>();
            }).CreateMapper();


            var entity = mapper.Map<User>(request);

            var createdEntity = await _dbContext.Set<User>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<User>(createdEntity.Entity);
        }
    }



}
