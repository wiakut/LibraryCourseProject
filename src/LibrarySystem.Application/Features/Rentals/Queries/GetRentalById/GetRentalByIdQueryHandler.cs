using AutoMapper;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetRentalById;
public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalTransactionDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetRentalByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<RentalTransactionDto?> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        var rental = await _context.RentalTransactions
            .Include(rt => rt.Book)
            .Include(rt => rt.Reader)
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Id == request.Id, cancellationToken);
        return rental == null ? null : _mapper.Map<RentalTransactionDto>(rental);
    }
}