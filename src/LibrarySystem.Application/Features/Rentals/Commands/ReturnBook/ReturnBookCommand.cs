using LibrarySystem.Application.Features.Rentals.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Commands.ReturnBook;
public record ReturnBookCommand(
    Guid RentalTransactionId,
    DateTime ActualReturnDate,
    decimal DamageCost
) : IRequest<RentalTransactionDto>;