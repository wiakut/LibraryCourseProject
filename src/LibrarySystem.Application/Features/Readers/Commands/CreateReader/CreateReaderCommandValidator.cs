using FluentValidation;
namespace LibrarySystem.Application.Features.Readers.Commands.CreateReader;
public class CreateReaderCommandValidator : AbstractValidator<CreateReaderCommand>
{
    public CreateReaderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters");
        RuleFor(x => x.ReaderCategoryId)
            .NotEmpty().WithMessage("Reader category is required");
    }
}