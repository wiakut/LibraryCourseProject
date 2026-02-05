using LibrarySystem.Application.Features.RentalRequests.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.DenyRentalRequest;
public record DenyRentalRequestCommand(
    Guid RentalRequestId,
    string DenialReason,
    string ProcessedByUserId
) : IRequest<RentalRequestDto>;