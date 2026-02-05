using MediatR;
namespace LibrarySystem.Application.Features.Books.Commands.DeleteBook;
public record DeleteBookCommand(Guid Id) : IRequest<Unit>;