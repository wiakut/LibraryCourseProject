using System.Security.Claims;
using LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId;
using LibrarySystem.Application.Features.Rentals.Queries.GetRentalById;
using LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByReaderId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Portal.Controllers;
[Area("Portal")]
[Authorize(Policy = "Reader")]
public class RentalsController : Controller
{
    private readonly IMediator _mediator;
    public RentalsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(string? status)
    {
        var readerId = await GetCurrentReaderIdAsync();
        if (readerId == null)
        {
            return Unauthorized();
        }
        var rentals = await _mediator.Send(new GetRentalsByReaderIdQuery(readerId.Value));
        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<LibrarySystem.Domain.Enums.RentalStatus>(status, out var parsedStatus))
        {
            rentals = rentals.Where(r => r.Status == parsedStatus).ToList();
        }
        ViewBag.StatusFilter = status;
        return View(rentals);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var readerId = await GetCurrentReaderIdAsync();
        if (readerId == null)
        {
            return Unauthorized();
        }
        var rental = await _mediator.Send(new GetRentalByIdQuery(id));
        if (rental == null || rental.ReaderId != readerId.Value)
        {
            return NotFound();
        }
        return View(rental);
    }
    private async Task<Guid?> GetCurrentReaderIdAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }
        var reader = await _mediator.Send(new GetReaderByUserIdQuery(userId));
        return reader?.Id;
    }
}