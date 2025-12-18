using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetAllRentalRequests;
public class GetAllRentalRequestsQueryHandler : IRequestHandler<GetAllRentalRequestsQuery, IEnumerable<RentalRequestDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllRentalRequestsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RentalRequestDto>> Handle(GetAllRentalRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .AsQueryable();
        if (request.Status.HasValue)
        {
            query = query.Where(rr => rr.Status == request.Status.Value);
        }
        var rentalRequests = await query
            .OrderByDescending(rr => rr.RequestDate)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RentalRequestDto>>(rentalRequests);
    }
}