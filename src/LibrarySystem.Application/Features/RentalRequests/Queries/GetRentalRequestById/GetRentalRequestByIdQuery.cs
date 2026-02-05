using LibrarySystem.Application.Features.RentalRequests.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestById;
public record GetRentalRequestByIdQuery(Guid Id) : IRequest<RentalRequestDto?>;