using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.CreateReaderCategory;
public record CreateReaderCategoryCommand(
    string Name,
    decimal DiscountPercentage
) : IRequest<ReaderCategoryDto>;