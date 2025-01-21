using EventsWebApplication.Application.DTOs.ParticipantDTOs;

namespace EventsWebApplication.Application.Validators
{
    public class ParticipantValidator : AbstractValidator<ParticipantRequestDto>
    {
        public ParticipantValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .MaximumLength(50).WithMessage("Фамилия не должна превышать 50 символов");

            RuleFor(p => p.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Дата рождения должна быть в прошлом")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Возраст должен быть не старше 120 лет");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный Email");
        }
    }
}
