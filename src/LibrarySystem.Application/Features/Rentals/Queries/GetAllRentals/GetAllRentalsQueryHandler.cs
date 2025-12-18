using AutoMapper;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetAllRentals;
public class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, List<RentalTransactionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllRentalsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<RentalTransactionDto>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
    {
        var rentals = await _context.RentalTransactions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<RentalTransactionDto>>(rentals);
    }
}