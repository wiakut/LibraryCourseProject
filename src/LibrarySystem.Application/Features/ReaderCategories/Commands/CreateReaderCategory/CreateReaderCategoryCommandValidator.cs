using FluentValidation;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.CreateReaderCategory;
public class CreateReaderCategoryCommandValidator : AbstractValidator<CreateReaderCategoryCommand>
{
    public CreateReaderCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        RuleFor(x => x.DiscountPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Discount percentage cannot be negative.")
            .LessThanOrEqualTo(100).WithMessage("Discount percentage cannot exceed 100%.");
    }
}