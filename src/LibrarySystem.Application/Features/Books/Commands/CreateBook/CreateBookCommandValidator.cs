using FluentValidation;
namespace LibrarySystem.Application.Features.Books.Commands.CreateBook;
public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(100).WithMessage("Author must not exceed 100 characters");
        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .MaximumLength(50).WithMessage("Genre must not exceed 50 characters");
        RuleFor(x => x.PledgeValue)
            .GreaterThan(0).WithMessage("Pledge value must be greater than 0");
        RuleFor(x => x.BaseRentalCost)
            .GreaterThan(0).WithMessage("Base rental cost must be greater than 0");
    }
}