using FluentValidation;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Validators
{
    public class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Название события обязательно")
                .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

            RuleFor(e => e.DateAndTime)
                .GreaterThan(DateTime.Now).WithMessage("Дата проведения не может быть в прошлом");

            RuleFor(e => e.MaxParticipants)
                .GreaterThan(0).WithMessage("Максимальное количество участников должно быть больше 0");

            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Место проведения обязательно");

            RuleFor(e => e.Category)
                .NotEmpty().WithMessage("Категория обязательна");
        }
    }
}
