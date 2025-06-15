using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class StatusReasonRequestDtoValidator : AbstractValidator<StatusReasonRequestDto>
    {
        public StatusReasonRequestDtoValidator()
        {
            RuleFor(request => request.StatusReason)
                .NotEmpty().WithMessage("Status reason is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Status reason cannot be whitespaces only.")
                .NotNull().WithMessage("Status reason is required.");
        }
    }
}
