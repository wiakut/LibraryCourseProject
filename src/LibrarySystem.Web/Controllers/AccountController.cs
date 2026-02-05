using LibrarySystem.Application.Features.Auth.Commands.Login;
using LibrarySystem.Application.Features.Auth.Commands.Register;
using LibrarySystem.Application.Features.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace LibrarySystem.Web.Controllers;
public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public AccountController(
        IMediator mediator,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "Portal" });
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "An error occurred during login.");
            return View(model);
        }
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var command = new RegisterCommand(
                model.Email,
                model.Password,
                model.Name,
                model.Address,
                model.Phone);
            var result = await _mediator.Send(command);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!)
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);
            }
            return RedirectToAction("Index", "Home", new { area = "Portal" });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
    [HttpPost]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await SignOutAsync();
        return RedirectToAction("Login");
    }
    private async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        Response.Cookies.Delete(".AspNetCore.Identity.Application");
        Response.Cookies.Delete(".AspNetCore.Cookies");
        foreach (var cookie in Request.Cookies.Keys)
        {
            if (cookie.Contains("Identity") || cookie.Contains("Auth"))
            {
                Response.Cookies.Delete(cookie);
            }
        }
    }
}