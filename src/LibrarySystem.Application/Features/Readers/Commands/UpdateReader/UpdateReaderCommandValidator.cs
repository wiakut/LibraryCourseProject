using FluentValidation;
namespace LibrarySystem.Application.Features.Readers.Commands.UpdateReader;
public class UpdateReaderCommandValidator : AbstractValidator<UpdateReaderCommand>
{
    public UpdateReaderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.ReaderCategoryId)
            .NotEmpty();
    }
}