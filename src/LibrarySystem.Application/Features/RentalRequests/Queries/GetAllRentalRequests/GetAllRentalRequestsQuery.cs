using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Domain.Enums;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetAllRentalRequests;
public record GetAllRentalRequestsQuery(RentalRequestStatus? Status = null) : IRequest<IEnumerable<RentalRequestDto>>;