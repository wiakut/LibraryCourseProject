using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Queries.GetRentalRequestById;
public class GetRentalRequestByIdQueryHandler : IRequestHandler<GetRentalRequestByIdQuery, RentalRequestDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetRentalRequestByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<RentalRequestDto?> Handle(GetRentalRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var rentalRequest = await _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .FirstOrDefaultAsync(rr => rr.Id == request.Id, cancellationToken);
        if (rentalRequest == null)
        {
            return null;
        }
        return _mapper.Map<RentalRequestDto>(rentalRequest);
    }
}