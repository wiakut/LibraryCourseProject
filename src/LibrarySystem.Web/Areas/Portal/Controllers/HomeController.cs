using System.Security.Claims;
using LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId;
using LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByReaderId;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Portal.Controllers;
[Area("Portal")]
[Authorize(Policy = "Reader")]
public class HomeController : Controller
{
    private readonly IMediator _mediator;
    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var reader = await _mediator.Send(new GetReaderByUserIdQuery(userId));
        if (reader == null)
        {
            return NotFound();
        }
        var rentals = await _mediator.Send(new GetRentalsByReaderIdQuery(reader.Id));
        var rentalRequests = await _mediator.Send(new GetRentalRequestsByReaderIdQuery(reader.Id));
        var activeRentals = rentals.Where(r => r.Status == RentalStatus.Active).ToList();
        var pendingRequests = rentalRequests.Where(r => r.Status == RentalRequestStatus.Pending).ToList();
        var overdueRentals = rentals.Where(r => r.Status == RentalStatus.Active && r.ExpectedReturnDate < DateTime.UtcNow).ToList();
        ViewBag.Reader = reader;
        ViewBag.ActiveRentalsCount = activeRentals.Count;
        ViewBag.PendingRequestsCount = pendingRequests.Count;
        ViewBag.TotalRentalsCount = rentals.Count;
        ViewBag.OverdueRentalsCount = overdueRentals.Count;
        ViewBag.RecentRentals = rentals.OrderByDescending(r => r.IssueDate).Take(5).ToList();
        ViewBag.RecentRequests = rentalRequests.OrderByDescending(r => r.RequestDate).Take(5).ToList();
        return View();
    }
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}