using FluentValidation;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.CreateRentalRequest;
public class CreateRentalRequestCommandValidator : AbstractValidator<CreateRentalRequestCommand>
{
    public CreateRentalRequestCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.");
        RuleFor(x => x.ReaderId)
            .NotEmpty().WithMessage("Reader ID is required.");
    }
}