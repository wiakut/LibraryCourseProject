using LibrarySystem.Application.Features.Auth.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace LibrarySystem.Application.Features.Auth.Commands.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    public LoginCommandHandler(
        UserManager<IdentityUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, roles);
        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = string.Empty, // Can be implemented with refresh token logic
            Email = user.Email!,
            Roles = roles.ToList()
        };
    }
}