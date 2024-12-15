using EventsWebApplication.Core.DTOs;
using FluentValidation;
namespace EventsWebApplication.Core.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Имя пользователя обязательно");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Пароль обязателен");
        }
    }
}
