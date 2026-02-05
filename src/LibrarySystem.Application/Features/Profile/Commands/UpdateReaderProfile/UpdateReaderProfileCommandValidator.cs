using FluentValidation;
namespace LibrarySystem.Application.Features.Profile.Commands.UpdateReaderProfile;
public class UpdateReaderProfileCommandValidator : AbstractValidator<UpdateReaderProfileCommand>
{
    public UpdateReaderProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(50);
    }
}