using LibrarySystem.Application.Features.Rentals.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetRentalById;
public record GetRentalByIdQuery(Guid Id) : IRequest<RentalTransactionDto?>;