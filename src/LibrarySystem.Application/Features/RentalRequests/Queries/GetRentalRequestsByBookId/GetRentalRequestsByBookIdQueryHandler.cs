using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByBookId;
public class GetRentalRequestsByBookIdQueryHandler : IRequestHandler<GetRentalRequestsByBookIdQuery, IEnumerable<RentalRequestDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetRentalRequestsByBookIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RentalRequestDto>> Handle(GetRentalRequestsByBookIdQuery request, CancellationToken cancellationToken)
    {
        var rentalRequests = await _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .Where(rr => rr.BookId == request.BookId)
            .OrderByDescending(rr => rr.RequestDate)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RentalRequestDto>>(rentalRequests);
    }
}