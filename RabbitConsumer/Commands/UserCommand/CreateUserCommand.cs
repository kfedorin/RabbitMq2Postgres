using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        public CreateUserCommandHandler(IDbContext dbContext, IMapper mapper) : base()
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var entity = _mapper.Map<User>(request);

            var createdEntity = await _dbContext.Set<User>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<User>(createdEntity.Entity);
        }

    }

}
