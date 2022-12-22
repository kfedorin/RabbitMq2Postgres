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

        public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserCommandValidator()
            {

                var msg = "Ошибка в поле {PropertyName}: значение {PropertyValue}";

                RuleFor(c => c.FirstName)
                    .NotNull().NotEmpty().Length(1, 20).WithMessage("Не может быть null, длина должна быть от {MinLength} до {MaxLength}");

                RuleFor(c => c.LastName)
                    .NotNull().NotEmpty().Length(1, 20).WithMessage("Не может быть null, длина должна быть от {MinLength} до {MaxLength}");

                RuleFor(c => c.MiddleName)
                    .NotNull().WithMessage("Не может быть null");

                RuleFor(c => c.Phone)
                    .NotEmpty()
                    .Must(IsPhoneValid).WithMessage(msg)
                    .Length(12).WithMessage("Длина должна быть от {MinLength} до {MaxLength}. Текущая длина: {TotalLength}");

                RuleFor(c => c.Email)
                    .NotNull().WithMessage(msg)
                    .EmailAddress();
            }

            private bool IsPhoneValid(string phone)
            {
                return !(!phone.StartsWith("+79")
                         || !phone.Substring(1).All(c => char.IsDigit(c)));
            }
        }
    }
}
