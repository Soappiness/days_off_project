using Domain.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class DayOffDtoValidator : AbstractValidator<DayOffCreateDto>
    {
        public DayOffDtoValidator()
        {
            RuleFor(dayOff => dayOff.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(dayOff => dayOff.EndDate)
                .NotEmpty().WithMessage("End date is required.");

            RuleFor(dayOff => dayOff.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Employee ID cannot be empty.");

            RuleFor(dayOff => dayOff.Type)
                .NotEmpty().WithMessage("Day off type is required.")
                .IsInEnum().WithMessage("Invalid day off type specified.");
        }
    }
}
