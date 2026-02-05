using LibrarySystem.Application.Features.Books.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Books.Commands.UpdateBook;
public record UpdateBookCommand(
    Guid Id,
    string Title,
    string Author,
    string Genre,
    decimal PledgeValue,
    decimal BaseRentalCost,
    int TotalCount
) : IRequest<BookDto>;