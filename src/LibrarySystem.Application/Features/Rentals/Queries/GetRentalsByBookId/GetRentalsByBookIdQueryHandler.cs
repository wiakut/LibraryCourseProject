using AutoMapper;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetRentalsByBookId;
public class GetRentalsByBookIdQueryHandler : IRequestHandler<GetRentalsByBookIdQuery, List<RentalTransactionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetRentalsByBookIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<RentalTransactionDto>> Handle(GetRentalsByBookIdQuery request, CancellationToken cancellationToken)
    {
        var rentals = await _context.RentalTransactions
            .Include(rt => rt.Book)
            .Include(rt => rt.Reader)
            .AsNoTracking()
            .Where(r => r.BookId == request.BookId)
            .OrderByDescending(rt => rt.IssueDate)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<RentalTransactionDto>>(rentals);
    }
}