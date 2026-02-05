using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Books.Commands.DeleteBook;
public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeleteBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.Id} not found.");
        }
        var hasActiveRentals = await _context.RentalTransactions
            .AnyAsync(rt => rt.BookId == request.Id && rt.Status == RentalStatus.Active, cancellationToken);
        if (hasActiveRentals)
        {
            throw new InvalidOperationException("Cannot delete book with active rentals.");
        }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}