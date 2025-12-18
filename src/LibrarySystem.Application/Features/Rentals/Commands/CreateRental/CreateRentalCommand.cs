using LibrarySystem.Application.Features.Rentals.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Commands.CreateRental;
public record CreateRentalCommand(
    Guid BookId,
    Guid ReaderId,
    DateTime ExpectedReturnDate
) : IRequest<RentalTransactionDto>;