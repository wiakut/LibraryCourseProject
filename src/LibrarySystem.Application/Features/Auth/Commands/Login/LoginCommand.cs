using LibrarySystem.Application.Features.Auth.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Auth.Commands.Login;
public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;