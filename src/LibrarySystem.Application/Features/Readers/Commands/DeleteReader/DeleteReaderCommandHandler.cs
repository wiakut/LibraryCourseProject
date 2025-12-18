using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Readers.Commands.DeleteReader;
public class DeleteReaderCommandHandler : IRequestHandler<DeleteReaderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeleteReaderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteReaderCommand request, CancellationToken cancellationToken)
    {
        var reader = await _context.Readers
            .Include(r => r.RentalTransactions)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (reader == null)
        {
            return Unit.Value;
        }
        var hasActiveRentals = reader.RentalTransactions.Any(r => r.Status == LibrarySystem.Domain.Enums.RentalStatus.Active);
        if (hasActiveRentals)
        {
            return Unit.Value;
        }
        _context.Readers.Remove(reader);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}