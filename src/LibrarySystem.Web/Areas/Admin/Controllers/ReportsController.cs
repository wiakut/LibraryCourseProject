using LibrarySystem.Application.Features.Reports.Queries.BookInventoryReport;
using LibrarySystem.Application.Features.Reports.Queries.FinancialReport;
using LibrarySystem.Application.Features.Reports.Queries.OverdueBooksReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class ReportsController : Controller
{
    private readonly IMediator _mediator;
    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> BookInventory()
    {
        var report = await _mediator.Send(new BookInventoryReportQuery());
        return View(report);
    }
    public async Task<IActionResult> OverdueBooks()
    {
        var report = await _mediator.Send(new OverdueBooksReportQuery());
        return View(report);
    }
    public async Task<IActionResult> Financial(DateTime? from, DateTime? to)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;
        if (from.HasValue)
        {
            startDate = from.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Utc)
                : from.Value.Date.ToUniversalTime();
        }
        if (to.HasValue)
        {
            var toDate = to.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(to.Value.Date, DateTimeKind.Utc)
                : to.Value.Date.ToUniversalTime();
            endDate = toDate.AddDays(1).AddTicks(-1); // End of day
        }
        var query = new FinancialReportQuery(startDate, endDate);
        var report = await _mediator.Send(query);
        ViewBag.From = from;
        ViewBag.To = to;
        return View(report);
    }
}