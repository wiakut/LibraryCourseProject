using LibrarySystem.Application.Features.RentalRequests.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByBookId;
public record GetRentalRequestsByBookIdQuery(Guid BookId) : IRequest<IEnumerable<RentalRequestDto>>;