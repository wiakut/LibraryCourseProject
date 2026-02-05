using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.ReaderCategories.Queries.GetAllReaderCategories;
public record GetAllReaderCategoriesQuery() : IRequest<IEnumerable<ReaderCategoryDto>>;