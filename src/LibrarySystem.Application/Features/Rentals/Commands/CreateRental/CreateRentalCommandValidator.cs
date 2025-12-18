using FluentValidation;
namespace LibrarySystem.Application.Features.Rentals.Commands.CreateRental;
public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
{
    public CreateRentalCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required");
        RuleFor(x => x.ReaderId)
            .NotEmpty().WithMessage("Reader ID is required");
        RuleFor(x => x.ExpectedReturnDate)
            .NotEmpty().WithMessage("Expected return date is required")
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Expected return date must be in the future");
    }
}