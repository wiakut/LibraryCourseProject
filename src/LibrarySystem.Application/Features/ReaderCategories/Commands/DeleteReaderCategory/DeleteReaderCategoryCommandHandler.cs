using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.DeleteReaderCategory;
public class DeleteReaderCategoryCommandHandler : IRequestHandler<DeleteReaderCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeleteReaderCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteReaderCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.ReaderCategories
            .Include(rc => rc.Readers)
            .FirstOrDefaultAsync(rc => rc.Id == request.Id, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Reader category with ID {request.Id} not found.");
        }
        if (category.Readers.Any())
        {
            throw new InvalidOperationException("Cannot delete a category that has readers assigned to it. Please reassign readers to another category first.");
        }
        _context.ReaderCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}