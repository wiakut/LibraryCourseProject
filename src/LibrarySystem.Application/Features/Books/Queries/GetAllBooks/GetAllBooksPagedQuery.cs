using LibrarySystem.Application.Common.Dtos;
using LibrarySystem.Application.Features.Books.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Books.Queries.GetAllBooks;
public record GetAllBooksPagedQuery(
    string? Search = null,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResultDto<BookDto>>;