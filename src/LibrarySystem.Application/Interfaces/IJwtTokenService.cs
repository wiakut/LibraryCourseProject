namespace LibrarySystem.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
    System.Security.Claims.ClaimsPrincipal? GetPrincipalFromToken(string token);
    bool ValidateToken(string token);
}
