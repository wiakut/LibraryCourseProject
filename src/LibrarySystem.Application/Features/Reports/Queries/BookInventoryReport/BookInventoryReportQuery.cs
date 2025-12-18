using LibrarySystem.Application.Features.Reports.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Reports.Queries.BookInventoryReport;
public record BookInventoryReportQuery() : IRequest<List<BookInventoryReportDto>>;