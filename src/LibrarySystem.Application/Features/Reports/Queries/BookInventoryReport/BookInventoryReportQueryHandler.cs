using LibrarySystem.Application.Features.Reports.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Reports.Queries.BookInventoryReport;
public class BookInventoryReportQueryHandler : IRequestHandler<BookInventoryReportQuery, List<BookInventoryReportDto>>
{
    private readonly IApplicationDbContext _context;
    public BookInventoryReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<BookInventoryReportDto>> Handle(BookInventoryReportQuery request, CancellationToken cancellationToken)
    {
        var books = await _context.Books
            .Select(book => new BookInventoryReportDto
            {
                BookId = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                IsAvailable = !_context.RentalTransactions
                    .Any(rt => rt.BookId == book.Id && rt.Status == RentalStatus.Active)
            })
            .ToListAsync(cancellationToken);
        return books;
    }
}