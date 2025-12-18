using LibrarySystem.Application.Features.Rentals.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByBookId;
public record GetRentalsByBookIdQuery(Guid BookId) : IRequest<List<RentalTransactionDto>>;