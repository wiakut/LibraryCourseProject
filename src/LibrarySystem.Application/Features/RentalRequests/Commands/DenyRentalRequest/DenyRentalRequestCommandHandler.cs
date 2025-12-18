using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.DenyRentalRequest;
public class DenyRentalRequestCommandHandler : IRequestHandler<DenyRentalRequestCommand, RentalRequestDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public DenyRentalRequestCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<RentalRequestDto> Handle(DenyRentalRequestCommand request, CancellationToken cancellationToken)
    {
        var rentalRequest = await _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .FirstOrDefaultAsync(rr => rr.Id == request.RentalRequestId, cancellationToken);
        if (rentalRequest == null)
        {
            throw new KeyNotFoundException($"Rental request with ID {request.RentalRequestId} not found.");
        }
        if (rentalRequest.Status != RentalRequestStatus.Pending)
        {
            throw new InvalidOperationException("Only pending rental requests can be denied.");
        }
        rentalRequest.Status = RentalRequestStatus.Denied;
        rentalRequest.DenialReason = request.DenialReason;
        rentalRequest.ProcessedDate = DateTime.UtcNow;
        rentalRequest.ProcessedByUserId = request.ProcessedByUserId;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RentalRequestDto>(rentalRequest);
    }
}