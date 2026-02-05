using System.Security.Claims;
using LibrarySystem.Application.Features.RentalRequests.Commands.ApproveRentalRequest;
using LibrarySystem.Application.Features.RentalRequests.Commands.DenyRentalRequest;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetAllRentalRequests;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestById;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByBookId;
using LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class RentalRequestsController : Controller
{
    private readonly IMediator _mediator;
    public RentalRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(RentalRequestStatus? status)
    {
        var query = new GetAllRentalRequestsQuery(status);
        var requests = await _mediator.Send(query);
        ViewBag.Status = status;
        return View(requests);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var query = new GetRentalRequestByIdQuery(id);
        var request = await _mediator.Send(query);
        if (request == null)
        {
            return NotFound();
        }
        return View(request);
    }
    public async Task<IActionResult> Approve(Guid id)
    {
        var query = new GetRentalRequestByIdQuery(id);
        var request = await _mediator.Send(query);
        if (request == null)
        {
            return NotFound();
        }
        if (request.Status != RentalRequestStatus.Pending)
        {
            TempData["ErrorMessage"] = "Only pending requests can be approved.";
            return RedirectToAction(nameof(Index));
        }
        var command = new ApproveRentalRequestCommand(
            request.Id,
            DateTime.UtcNow.AddDays(14), // Default 14 days
            GetCurrentUserId() ?? string.Empty);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(ApproveRentalRequestCommand command)
    {
        if (ModelState.IsValid)
        {
            try
            {
                command = command with { ProcessedByUserId = GetCurrentUserId() ?? string.Empty };
                await _mediator.Send(command);
                TempData["SuccessMessage"] = "Rental request approved successfully. Rental transaction created.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(command);
    }
    public async Task<IActionResult> Deny(Guid id)
    {
        var query = new GetRentalRequestByIdQuery(id);
        var request = await _mediator.Send(query);
        if (request == null)
        {
            return NotFound();
        }
        if (request.Status != RentalRequestStatus.Pending)
        {
            TempData["ErrorMessage"] = "Only pending requests can be denied.";
            return RedirectToAction(nameof(Index));
        }
        var command = new DenyRentalRequestCommand(
            request.Id,
            string.Empty,
            GetCurrentUserId() ?? string.Empty);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deny(DenyRentalRequestCommand command)
    {
        if (ModelState.IsValid)
        {
            try
            {
                command = command with { ProcessedByUserId = GetCurrentUserId() ?? string.Empty };
                await _mediator.Send(command);
                TempData["SuccessMessage"] = "Rental request denied successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(command);
    }
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}