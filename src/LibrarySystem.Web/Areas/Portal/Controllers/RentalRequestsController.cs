using System.Security.Claims;
using LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId;
using LibrarySystem.Application.Features.RentalRequests.Commands.CreateRentalRequest;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestById;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Portal.Controllers;
[Area("Portal")]
[Authorize(Policy = "Reader")]
public class RentalRequestsController : Controller
{
    private readonly IMediator _mediator;
    public RentalRequestsController(IMediator mediator)
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
        var query = new GetRentalRequestsByReaderIdQuery(reader.Id);
        var requests = await _mediator.Send(query);
        return View(requests);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid bookId)
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
        try
        {
            var command = new CreateRentalRequestCommand(bookId, reader.Id);
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Rental request created successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Details", "Books", new { id = bookId });
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var query = new GetRentalRequestByIdQuery(id);
        var request = await _mediator.Send(query);
        if (request == null)
        {
            return NotFound();
        }
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var reader = await _mediator.Send(new GetReaderByUserIdQuery(userId));
        if (reader == null || reader.Id != request.ReaderId)
        {
            return Forbid();
        }
        return View(request);
    }
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}