using LibrarySystem.Application.Features.Auth.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Auth.Commands.Register;
public record RegisterCommand(
    string Email,
    string Password,
    string Name,
    string Address,
    string Phone
) : IRequest<AuthResponseDto>;