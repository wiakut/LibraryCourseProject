using LibrarySystem.Application.Features.Reports.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Reports.Queries.OverdueBooksReport;
public record OverdueBooksReportQuery() : IRequest<List<OverdueBooksReportDto>>;