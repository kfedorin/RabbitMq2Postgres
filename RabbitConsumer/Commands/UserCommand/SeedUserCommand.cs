using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.OrganizationCommand
{
    public class SeedUserCommand : IRequest<User>
    {
    

    }

    public class SeedUserCommandHandler : IRequestHandler<SeedUserCommand, User>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        public SeedUserCommandHandler(IDbContext dbContext, IMapper mapper) : base()
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<User> Handle(SeedUserCommand request, CancellationToken cancellationToken)
        {
            Random random = new Random();

            var user = new User
            {
                FirstName = "FirstName_" + random.Next(1, 100),
                LastName = "LastName_" + random.Next(1, 100),
                MiddleName = "MiddleName_" + random.Next(1, 100),
                Email = "nameMail_" + random.Next(0, 100) + "@email.ru",
                Phone = "+7987654321" + random.Next(0, 9)
            };

            var createdEntity = await _dbContext.Set<User>().AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<User>(createdEntity.Entity); ;
        }
    }
}
