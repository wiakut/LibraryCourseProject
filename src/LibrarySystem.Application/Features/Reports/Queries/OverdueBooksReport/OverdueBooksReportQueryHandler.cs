using LibrarySystem.Application.Features.Reports.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Reports.Queries.OverdueBooksReport;
public class OverdueBooksReportQueryHandler : IRequestHandler<OverdueBooksReportQuery, List<OverdueBooksReportDto>>
{
    private readonly IApplicationDbContext _context;
    private const decimal DailyFineRate = 2.0m;
    public OverdueBooksReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<OverdueBooksReportDto>> Handle(OverdueBooksReportQuery request, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.UtcNow;
        var overdueRentals = await _context.RentalTransactions
            .Include(rt => rt.Book)
            .Include(rt => rt.Reader)
            .Where(rt => rt.Status == RentalStatus.Active && rt.ExpectedReturnDate < currentDate)
            .Select(rt => new OverdueBooksReportDto
            {
                RentalTransactionId = rt.Id,
                BookTitle = rt.Book.Title,
                BookAuthor = rt.Book.Author,
                ReaderName = rt.Reader.Name,
                ReaderPhone = rt.Reader.Phone,
                IssueDate = rt.IssueDate,
                ExpectedReturnDate = rt.ExpectedReturnDate,
                OverdueDays = (int)(currentDate - rt.ExpectedReturnDate).TotalDays,
                EstimatedFine = (decimal)(currentDate - rt.ExpectedReturnDate).TotalDays * DailyFineRate
            })
            .OrderByDescending(r => r.OverdueDays)
            .ToListAsync(cancellationToken);
        return overdueRentals;
    }
}