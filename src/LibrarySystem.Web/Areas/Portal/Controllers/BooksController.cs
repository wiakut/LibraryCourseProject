using System.Security.Claims;
using LibrarySystem.Application.Features.Books.Queries.GetAllBooks;
using LibrarySystem.Application.Features.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Portal.Controllers;
[Area("Portal")]
[Authorize(Policy = "Reader")]
public class BooksController : Controller
{
    private readonly IMediator _mediator;
    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(string? search, string? genre)
    {
        var query = new GetAllBooksPagedQuery(Search: search, PageNumber: 1, PageSize: 12);
        var result = await _mediator.Send(query);
        var books = result.Items;
        if (!string.IsNullOrWhiteSpace(genre))
        {
            books = books.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        ViewBag.Search = search;
        ViewBag.Genre = genre;
        var allBooksQuery = new GetAllBooksQuery();
        var allBooks = await _mediator.Send(allBooksQuery);
        var genres = allBooks
            .Select(b => b.Genre)
            .Distinct()
            .OrderBy(g => g)
            .ToList();
        ViewBag.Genres = genres;
        return View(books);
    }
    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] string? search, [FromQuery] string? genre, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
    {
        var query = new GetAllBooksPagedQuery(Search: search, PageNumber: pageNumber, PageSize: pageSize);
        var result = await _mediator.Send(query);
        if (!string.IsNullOrWhiteSpace(genre))
        {
            result.Items = result.Items.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            result.TotalCount = result.Items.Count;
        }
        return Json(result);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var book = await _mediator.Send(new GetBookByIdQuery(id));
        if (book == null)
        {
            return NotFound();
        }
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        bool hasPendingRequest = false;
        if (userId != null)
        {
            var reader = await _mediator.Send(new LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId.GetReaderByUserIdQuery(userId));
            if (reader != null)
            {
                var requests = await _mediator.Send(new LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId.GetRentalRequestsByReaderIdQuery(reader.Id));
                hasPendingRequest = requests.Any(r => r.BookId == id && r.Status == LibrarySystem.Domain.Enums.RentalRequestStatus.Pending);
            }
        }
        ViewBag.HasPendingRequest = hasPendingRequest;
        return View(book);
    }
}