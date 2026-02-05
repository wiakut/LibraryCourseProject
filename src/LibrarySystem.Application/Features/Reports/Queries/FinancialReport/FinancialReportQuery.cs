using LibrarySystem.Application.Features.Reports.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Reports.Queries.FinancialReport;
public record FinancialReportQuery(DateTime? StartDate, DateTime? EndDate) : IRequest<FinancialReportDto>;