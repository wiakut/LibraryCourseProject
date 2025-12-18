using LibrarySystem.Application.Features.Books.Commands.CreateBook;
using LibrarySystem.Application.Features.Books.Commands.DeleteBook;
using LibrarySystem.Application.Features.Books.Commands.UpdateBook;
using LibrarySystem.Application.Features.Books.Queries.GetAllBooks;
using LibrarySystem.Application.Features.Books.Queries.GetBookById;
using LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByBookId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class BooksController : Controller
{
    private readonly IMediator _mediator;
    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index()
    {
        var query = new GetAllBooksPagedQuery(Search: null, PageNumber: 1, PageSize: 20);
        var result = await _mediator.Send(query);
        return View(result.Items);
    }
    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetAllBooksPagedQuery(Search: search, PageNumber: pageNumber, PageSize: pageSize);
        var result = await _mediator.Send(query);
        return Json(result);
    }
    [HttpGet]
    public async Task<IActionResult> SearchBooks([FromQuery] string? search, [FromQuery] int limit = 10)
    {
        var query = new GetAllBooksPagedQuery(Search: search, PageNumber: 1, PageSize: limit);
        var result = await _mediator.Send(query);
        var suggestions = result.Items.Select(b => new { id = b.Id, text = $"{b.Title} by {b.Author}" }).ToList();
        return Json(suggestions);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        try
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var query = new GetBookByIdQuery(id);
        var book = await _mediator.Send(query);
        if (book == null)
        {
            return NotFound();
        }
        if (User.IsInRole("Admin"))
        {
            var rentalsQuery = new GetRentalsByBookIdQuery(id);
            var rentals = await _mediator.Send(rentalsQuery);
            ViewBag.Rentals = rentals;
            var rentalRequestsQuery = new LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByBookId.GetRentalRequestsByBookIdQuery(id);
            var rentalRequests = await _mediator.Send(rentalRequestsQuery);
            ViewBag.RentalRequests = rentalRequests;
        }
        return View(book);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var query = new GetBookByIdQuery(id);
        var book = await _mediator.Send(query);
        if (book == null)
        {
            return NotFound();
        }
        var command = new UpdateBookCommand(
            book.Id,
            book.Title,
            book.Author,
            book.Genre,
            book.PledgeValue,
            book.BaseRentalCost,
            book.TotalCount);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateBookCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        try
        {
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var command = new DeleteBookCommand(id);
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}