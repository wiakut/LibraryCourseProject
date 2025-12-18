using LibrarySystem.Application.Features.Books.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Books.Queries.GetBookById;
public record GetBookByIdQuery(Guid Id) : IRequest<BookDto?>;