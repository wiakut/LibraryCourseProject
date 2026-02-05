using FluentValidation;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.DenyRentalRequest;
public class DenyRentalRequestCommandValidator : AbstractValidator<DenyRentalRequestCommand>
{
    public DenyRentalRequestCommandValidator()
    {
        RuleFor(x => x.RentalRequestId)
            .NotEmpty().WithMessage("Rental request ID is required.");
        RuleFor(x => x.DenialReason)
            .NotEmpty().WithMessage("Denial reason is required.")
            .MaximumLength(500).WithMessage("Denial reason must not exceed 500 characters.");
        RuleFor(x => x.ProcessedByUserId)
            .NotEmpty().WithMessage("Processed by user ID is required.");
    }
}