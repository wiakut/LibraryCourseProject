using LibrarySystem.Application.Features.Readers.Commands.CreateReader;
using LibrarySystem.Application.Features.Readers.Commands.DeleteReader;
using LibrarySystem.Application.Features.Readers.Commands.UpdateReader;
using LibrarySystem.Application.Features.Readers.Queries.GetAllReaders;
using LibrarySystem.Application.Features.Readers.Queries.GetReaderById;
using LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByReaderId;
using LibrarySystem.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class ReadersController : Controller
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;
    public ReadersController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
    public async Task<IActionResult> Index()
    {
        var query = new GetAllReadersPagedQuery(Search: null, PageNumber: 1, PageSize: 20);
        var result = await _mediator.Send(query);
        return View(result.Items);
    }
    [HttpGet]
    public async Task<IActionResult> GetReaders([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetAllReadersPagedQuery(Search: search, PageNumber: pageNumber, PageSize: pageSize);
        var result = await _mediator.Send(query);
        return Json(result);
    }
    [HttpGet]
    public async Task<IActionResult> SearchReaders([FromQuery] string? search, [FromQuery] int limit = 10)
    {
        var query = new GetAllReadersPagedQuery(Search: search, PageNumber: 1, PageSize: limit);
        var result = await _mediator.Send(query);
        var suggestions = result.Items.Select(r => new { id = r.Id, text = r.Name }).ToList();
        return Json(suggestions);
    }
    [HttpGet]
    public async Task<IActionResult> SearchReaderCategories([FromQuery] string? search, [FromQuery] int limit = 10)
    {
        var query = _dbContext.ReaderCategories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(searchLower));
        }
        var categories = await query
            .OrderBy(c => c.Name)
            .Take(limit)
            .Select(c => new { id = c.Id, text = c.Name })
            .ToListAsync();
        return Json(categories);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var reader = await _mediator.Send(new GetReaderByIdQuery(id));
        if (reader == null)
        {
            return NotFound();
        }
        if (User.IsInRole("Admin"))
        {
            var rentalsQuery = new GetRentalsByReaderIdQuery(id);
            var rentals = await _mediator.Send(rentalsQuery);
            ViewBag.Rentals = rentals;
            var rentalRequestsQuery = new LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId.GetRentalRequestsByReaderIdQuery(id);
            var rentalRequests = await _mediator.Send(rentalRequestsQuery);
            var pendingRequests = rentalRequests.Where(rr => rr.Status == LibrarySystem.Domain.Enums.RentalRequestStatus.Pending).ToList();
            ViewBag.PendingRentalRequests = pendingRequests;
        }
        return View(reader);
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.ReaderCategories = await _dbContext.ReaderCategories.AsNoTracking().ToListAsync();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReaderCommand command)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReaderCategories = await _dbContext.ReaderCategories.AsNoTracking().ToListAsync();
            return View(command);
        }
        await _mediator.Send(command);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var reader = await _mediator.Send(new GetReaderByIdQuery(id));
        if (reader == null)
        {
            return NotFound();
        }
        ViewBag.ReaderCategories = await _dbContext.ReaderCategories.AsNoTracking().ToListAsync();
        var command = new UpdateReaderCommand(
            reader.Id,
            reader.Name,
            reader.Address,
            reader.Phone,
            reader.ReaderCategoryId);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateReaderCommand command)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReaderCategories = await _dbContext.ReaderCategories.AsNoTracking().ToListAsync();
            return View(command);
        }
        var updated = await _mediator.Send(command);
        if (updated == null)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteReaderCommand(id));
        return RedirectToAction(nameof(Index));
    }
}