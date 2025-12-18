using LibrarySystem.Application.Features.Rentals.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetAllRentals;
public record GetAllRentalsQuery() : IRequest<List<RentalTransactionDto>>;