using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Features.Rentals.Commands.CreateRental;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Pricing;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.ApproveRentalRequest;
public class ApproveRentalRequestCommandHandler : IRequestHandler<ApproveRentalRequestCommand, RentalRequestDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly PricingStrategyFactory _strategyFactory;
    public ApproveRentalRequestCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IMediator mediator,
        PricingStrategyFactory strategyFactory)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
        _strategyFactory = strategyFactory;
    }
    public async Task<RentalRequestDto> Handle(ApproveRentalRequestCommand request, CancellationToken cancellationToken)
    {
        var rentalRequest = await _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .ThenInclude(r => r.ReaderCategory)
            .FirstOrDefaultAsync(rr => rr.Id == request.RentalRequestId, cancellationToken);
        if (rentalRequest == null)
        {
            throw new KeyNotFoundException($"Rental request with ID {request.RentalRequestId} not found.");
        }
        if (rentalRequest.Status != RentalRequestStatus.Pending)
        {
            throw new InvalidOperationException("Only pending rental requests can be approved.");
        }
        if (rentalRequest.Book.AvailableCount <= 0)
        {
            throw new InvalidOperationException("Cannot approve request. Book is no longer available.");
        }
        var issueDate = DateTime.UtcNow;
        var expectedReturnDateUtc = request.ExpectedReturnDate.Kind == DateTimeKind.Utc
            ? request.ExpectedReturnDate
            : DateTime.SpecifyKind(request.ExpectedReturnDate, DateTimeKind.Utc);
        if (expectedReturnDateUtc <= issueDate)
        {
            throw new ArgumentException("Expected return date must be in the future.");
        }
        rentalRequest.Status = RentalRequestStatus.Approved;
        rentalRequest.ExpectedReturnDate = expectedReturnDateUtc;
        rentalRequest.ProcessedDate = DateTime.UtcNow;
        rentalRequest.ProcessedByUserId = request.ProcessedByUserId;
        var createRentalCommand = new CreateRentalCommand(
            rentalRequest.BookId,
            rentalRequest.ReaderId,
            expectedReturnDateUtc);
        await _mediator.Send(createRentalCommand, cancellationToken);
        rentalRequest.Book.AvailableCount--;
        rentalRequest.Book.InRentCount++;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RentalRequestDto>(rentalRequest);
    }
}