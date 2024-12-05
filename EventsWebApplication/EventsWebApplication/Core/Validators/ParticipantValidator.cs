using FluentValidation;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Core.Validators
{
    public class ParticipantValidator : AbstractValidator<Participant>
    {
        public ParticipantValidator()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("Имя обязательно");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна");
            RuleFor(e => e.Email)
                .EmailAddress().WithMessage("Некорректный Email");
            RuleFor(e => e.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Дата рождения должна быть в прошлом");
            RuleFor(e => e.EventId)
                .GreaterThan(0).WithMessage("Неверный идентификатор события");
        }
    }
}
