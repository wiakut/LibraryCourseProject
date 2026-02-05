using LibrarySystem.Application.Features.Auth.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Auth.Commands.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    public RegisterCommandHandler(
        UserManager<IdentityUser> userManager,
        IApplicationDbContext context,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _context = context;
        _jwtTokenService = jwtTokenService;
    }
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        await _userManager.AddToRoleAsync(user, UserRole.Reader);
        var defaultCategory = await _context.ReaderCategories
            .FirstOrDefaultAsync(c => c.Name == "Standard", cancellationToken);
        if (defaultCategory == null)
        {
            defaultCategory = new ReaderCategory
            {
                Id = Guid.NewGuid(),
                Name = "Standard",
                DiscountPercentage = 0
            };
            _context.ReaderCategories.Add(defaultCategory);
            await _context.SaveChangesAsync(cancellationToken);
        }
        var reader = new Reader
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            ReaderCategoryId = defaultCategory.Id,
            UserId = user.Id
        };
        _context.Readers.Add(reader);
        await _context.SaveChangesAsync(cancellationToken);
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, roles);
        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = string.Empty,
            Email = user.Email!,
            Roles = roles.ToList()
        };
    }
}