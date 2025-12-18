using LibrarySystem.Application.Features.RentalRequests.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId;
public record GetRentalRequestsByReaderIdQuery(Guid ReaderId) : IRequest<IEnumerable<RentalRequestDto>>;