using EventsWebApplication.Core.DTOs;
using FluentValidation;

namespace EventsWebApplication.Core.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Имя пользователя обязательно")
                .MaximumLength(50).WithMessage("Имя пользователя не должно превышать 50 символов");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .Must(p => p.Length >= 8).WithMessage("Пароль должен содержать не менее 8 символов")
                .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
                .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру");

            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("Роль обязательна")
                .Must(role => new[] { "Admin", "User" }.Contains(role))
                .WithMessage("Роль должна быть Admin или User");
        }
    }
}
