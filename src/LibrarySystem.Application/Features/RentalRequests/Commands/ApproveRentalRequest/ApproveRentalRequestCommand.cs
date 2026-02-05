using LibrarySystem.Application.Features.RentalRequests.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.ApproveRentalRequest;
public record ApproveRentalRequestCommand(
    Guid RentalRequestId,
    DateTime ExpectedReturnDate,
    string ProcessedByUserId
) : IRequest<RentalRequestDto>;