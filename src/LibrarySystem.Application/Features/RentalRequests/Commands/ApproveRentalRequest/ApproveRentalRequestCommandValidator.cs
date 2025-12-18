using FluentValidation;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.ApproveRentalRequest;
public class ApproveRentalRequestCommandValidator : AbstractValidator<ApproveRentalRequestCommand>
{
    public ApproveRentalRequestCommandValidator()
    {
        RuleFor(x => x.RentalRequestId)
            .NotEmpty().WithMessage("Rental request ID is required.");
        RuleFor(x => x.ExpectedReturnDate)
            .NotEmpty().WithMessage("Expected return date is required.")
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Expected return date must be in the future.");
        RuleFor(x => x.ProcessedByUserId)
            .NotEmpty().WithMessage("Processed by user ID is required.");
    }
}