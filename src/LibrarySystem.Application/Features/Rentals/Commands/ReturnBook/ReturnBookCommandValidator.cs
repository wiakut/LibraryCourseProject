using FluentValidation;
namespace LibrarySystem.Application.Features.Rentals.Commands.ReturnBook;
public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(x => x.RentalTransactionId)
            .NotEmpty().WithMessage("Rental transaction ID is required");
        RuleFor(x => x.ActualReturnDate)
            .NotEmpty().WithMessage("Actual return date is required");
        RuleFor(x => x.DamageCost)
            .GreaterThanOrEqualTo(0).WithMessage("Damage cost cannot be negative");
    }
}