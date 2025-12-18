using LibrarySystem.Application.Common.Dtos;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Domain.Enums;
using MediatR;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetAllRentals;
public record GetAllRentalsPagedQuery(
    string? Search = null,
    RentalStatus? Status = null,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResultDto<RentalTransactionDto>>;