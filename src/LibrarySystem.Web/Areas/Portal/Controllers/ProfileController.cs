using System.Security.Claims;
using LibrarySystem.Application.Features.Profile.Commands.UpdateReaderProfile;
using LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Portal.Controllers;
[Area("Portal")]
[Authorize(Policy = "Reader")]
public class ProfileController : Controller
{
    private readonly IMediator _mediator;
    public ProfileController(IMediator mediator)
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
        return View(reader);
    }
    public async Task<IActionResult> Edit()
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
        var command = new UpdateReaderProfileCommand(
            userId,
            reader.Name,
            reader.Address,
            reader.Phone);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateReaderProfileCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        command = command with { UserId = userId };
        await _mediator.Send(command);
        TempData["SuccessMessage"] = "Profile updated successfully.";
        return RedirectToAction(nameof(Index));
    }
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}