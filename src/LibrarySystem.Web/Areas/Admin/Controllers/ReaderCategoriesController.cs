using LibrarySystem.Application.Features.ReaderCategories.Commands.CreateReaderCategory;
using LibrarySystem.Application.Features.ReaderCategories.Commands.DeleteReaderCategory;
using LibrarySystem.Application.Features.ReaderCategories.Commands.UpdateReaderCategory;
using LibrarySystem.Application.Features.ReaderCategories.Queries.GetAllReaderCategories;
using LibrarySystem.Application.Features.ReaderCategories.Queries.GetReaderCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class ReaderCategoriesController : Controller
{
    private readonly IMediator _mediator;
    public ReaderCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index()
    {
        var query = new GetAllReaderCategoriesQuery();
        var categories = await _mediator.Send(query);
        return View(categories);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReaderCategoryCommand command)
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
    public async Task<IActionResult> Edit(Guid id)
    {
        var query = new GetReaderCategoryByIdQuery(id);
        var category = await _mediator.Send(query);
        if (category == null)
        {
            return NotFound();
        }
        var command = new UpdateReaderCategoryCommand(
            category.Id,
            category.Name,
            category.DiscountPercentage);
        return View(command);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateReaderCategoryCommand command)
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
            var command = new DeleteReaderCategoryCommand(id);
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