using LibrarySystem.Application.Features.Rentals.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByReaderId;
public record GetRentalsByReaderIdQuery(Guid ReaderId) : IRequest<List<RentalTransactionDto>>;