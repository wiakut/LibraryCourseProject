using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestsByReaderId;
public class GetRentalRequestsByReaderIdQueryHandler : IRequestHandler<GetRentalRequestsByReaderIdQuery, IEnumerable<RentalRequestDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetRentalRequestsByReaderIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RentalRequestDto>> Handle(GetRentalRequestsByReaderIdQuery request, CancellationToken cancellationToken)
    {
        var rentalRequests = await _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .Where(rr => rr.ReaderId == request.ReaderId)
            .OrderByDescending(rr => rr.RequestDate)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RentalRequestDto>>(rentalRequests);
    }
}