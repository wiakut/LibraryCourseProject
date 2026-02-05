using LibrarySystem.Application.Features.RentalRequests.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.CreateRentalRequest;
public record CreateRentalRequestCommand(
    Guid BookId,
    Guid ReaderId
) : IRequest<RentalRequestDto>;