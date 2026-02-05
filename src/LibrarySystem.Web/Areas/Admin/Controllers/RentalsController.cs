using LibrarySystem.Application.Features.Rentals.Commands.CreateRental;
using LibrarySystem.Application.Features.Rentals.Commands.ReturnBook;
using LibrarySystem.Application.Features.Rentals.Queries.GetAllRentals;
using LibrarySystem.Application.Features.Rentals.Queries.GetRentalById;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class RentalsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;
    public RentalsController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
    public async Task<IActionResult> Index(string? status)
    {
        RentalStatus? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<RentalStatus>(status, out var parsedStatus))
        {
            statusFilter = parsedStatus;
        }
        var query = new GetAllRentalsPagedQuery(Search: null, Status: statusFilter, PageNumber: 1, PageSize: 20);
        var result = await _mediator.Send(query);
        ViewBag.StatusFilter = status;
        return View(result.Items);
    }
    [HttpGet]
    public async Task<IActionResult> GetRentals([FromQuery] string? search, [FromQuery] string? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        RentalStatus? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<RentalStatus>(status, out var parsedStatus))
        {
            statusFilter = parsedStatus;
        }
        var query = new GetAllRentalsPagedQuery(Search: search, Status: statusFilter, PageNumber: pageNumber, PageSize: pageSize);
        var result = await _mediator.Send(query);
        return Json(result);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var rental = await _mediator.Send(new GetRentalByIdQuery(id));
        if (rental == null)
        {
            return NotFound();
        }
        return View(rental);
    }
    public async Task<IActionResult> Create()
    {
        await PopulateBooksAndReadersAsync();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRentalCommand command)
    {
        if (!ModelState.IsValid)
        {
            await PopulateBooksAndReadersAsync();
            return View(command);
        }
        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Rental created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateBooksAndReadersAsync();
            return View(command);
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateBooksAndReadersAsync();
            return View(command);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateBooksAndReadersAsync();
            return View(command);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while creating the rental. Please try again.");
            await PopulateBooksAndReadersAsync();
            return View(command);
        }
    }
    public async Task<IActionResult> Return(Guid id)
    {
        var rental = await _mediator.Send(new GetRentalByIdQuery(id));
        if (rental == null)
        {
            return NotFound();
        }
        var command = new ReturnBookCommand(
            rental.Id,
            DateTime.UtcNow,
            0m);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(ReturnBookCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        try
        {
            var result = await _mediator.Send(command);
            TempData["ReturnSummary"] = $"Fine: {result.FineAmount:C}, Refund: {result.RefundAmount:C}";
            TempData["SuccessMessage"] = "Book returned successfully.";
            return RedirectToAction(nameof(Details), new { id = result.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while returning the book. Please try again.");
            return View(command);
        }
    }
    private async Task PopulateBooksAndReadersAsync()
    {
        ViewBag.Books = await _dbContext.Books
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();
        ViewBag.Readers = await _dbContext.Readers
            .Include(r => r.ReaderCategory)
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync();
    }
}