using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.ReaderCategories.Queries.GetReaderCategoryById;
public record GetReaderCategoryByIdQuery(Guid Id) : IRequest<ReaderCategoryDto?>;