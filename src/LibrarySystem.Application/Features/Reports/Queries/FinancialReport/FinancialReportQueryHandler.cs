using LibrarySystem.Application.Features.Reports.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Reports.Queries.FinancialReport;
public class FinancialReportQueryHandler : IRequestHandler<FinancialReportQuery, FinancialReportDto>
{
    private readonly IApplicationDbContext _context;
    public FinancialReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<FinancialReportDto> Handle(FinancialReportQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate.HasValue
            ? (request.StartDate.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc)
                : request.StartDate.Value.ToUniversalTime())
            : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        var endDate = request.EndDate.HasValue
            ? (request.EndDate.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc)
                : request.EndDate.Value.ToUniversalTime())
            : DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
        var totalRentalIncome = await _context.RentalTransactions
            .Where(rt => rt.Status == RentalStatus.Returned &&
                        rt.ActualReturnDate.HasValue &&
                        rt.ActualReturnDate >= startDate &&
                        rt.ActualReturnDate <= endDate)
            .SumAsync(rt => rt.FinalRentalCost, cancellationToken);
        var totalFinesCollected = await _context.RentalTransactions
            .Where(rt => rt.Status == RentalStatus.Returned &&
                        rt.ActualReturnDate.HasValue &&
                        rt.ActualReturnDate >= startDate &&
                        rt.ActualReturnDate <= endDate)
            .SumAsync(rt => rt.FineAmount, cancellationToken);
        var totalPledgeFunds = await _context.RentalTransactions
            .Where(rt => rt.Status == RentalStatus.Active)
            .SumAsync(rt => rt.PledgeAmount, cancellationToken);
        var totalRefunds = await _context.RentalTransactions
            .Where(rt => rt.Status == RentalStatus.Returned &&
                        rt.ActualReturnDate.HasValue &&
                        rt.ActualReturnDate >= startDate &&
                        rt.ActualReturnDate <= endDate &&
                        rt.RefundAmount > 0)
            .SumAsync(rt => rt.RefundAmount, cancellationToken);
        var netIncome = totalRentalIncome + totalFinesCollected - totalRefunds;
        return new FinancialReportDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalRentalIncome = totalRentalIncome,
            TotalFinesCollected = totalFinesCollected,
            TotalPledgeFunds = totalPledgeFunds,
            TotalRefunds = totalRefunds,
            NetIncome = netIncome
        };
    }
}