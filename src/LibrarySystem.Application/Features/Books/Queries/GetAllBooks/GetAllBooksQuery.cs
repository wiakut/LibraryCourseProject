using LibrarySystem.Application.Features.Books.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Books.Queries.GetAllBooks;
public record GetAllBooksQuery() : IRequest<List<BookDto>>;