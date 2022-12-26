using FluentValidation;
using RabbitModel;

namespace RabbitProducer.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
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
