using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.UpdateReaderCategory;
public record UpdateReaderCategoryCommand(
    Guid Id,
    string Name,
    decimal DiscountPercentage
) : IRequest<ReaderCategoryDto>;