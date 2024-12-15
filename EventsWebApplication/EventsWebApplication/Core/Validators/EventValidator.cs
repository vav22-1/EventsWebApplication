using FluentValidation;
using EventsWebApplication.Core.DTOs;

namespace EventsWebApplication.Core.Validators
{
    public class EventValidator : AbstractValidator<EventRequestDto>
    {
        public EventValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Название события обязательно")
                .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

            RuleFor(e => e.Description)
                .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов");

            RuleFor(e => e.DateAndTime)
                .GreaterThan(DateTime.Now).WithMessage("Дата проведения не может быть в прошлом");

            RuleFor(e => e.MaxParticipants)
                .GreaterThan(0).WithMessage("Максимальное количество участников должно быть больше 0")
                .LessThanOrEqualTo(1000).WithMessage("Максимальное количество участников не должно превышать 1000");

            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Место проведения обязательно")
                .MaximumLength(200).WithMessage("Место проведения не должно превышать 200 символов");

            RuleFor(e => e.Category)
                .NotEmpty().WithMessage("Категория обязательна")
                .MaximumLength(50).WithMessage("Категория не должна превышать 50 символов");

        }
    }
}
