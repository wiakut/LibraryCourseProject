using AutoMapper;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.RentalRequests.Commands.CreateRentalRequest;
public class CreateRentalRequestCommandHandler : IRequestHandler<CreateRentalRequestCommand, RentalRequestDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateRentalRequestCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<RentalRequestDto> Handle(CreateRentalRequestCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.BookId} not found.");
        }
        if (book.AvailableCount <= 0)
        {
            throw new InvalidOperationException("Book is not available for rental. No copies available.");
        }
        var reader = await _context.Readers.FindAsync(new object[] { request.ReaderId }, cancellationToken);
        if (reader == null)
        {
            throw new KeyNotFoundException($"Reader with ID {request.ReaderId} not found.");
        }
        var existingRequest = await _context.RentalRequests
            .FirstOrDefaultAsync(
                rr => rr.BookId == request.BookId
                    && rr.ReaderId == request.ReaderId
                    && rr.Status == RentalRequestStatus.Pending,
                cancellationToken);
        if (existingRequest != null)
        {
            throw new InvalidOperationException("You already have a pending rental request for this book.");
        }
        var rentalRequest = new RentalRequest
        {
            Id = Guid.NewGuid(),
            BookId = request.BookId,
            ReaderId = request.ReaderId,
            RequestDate = DateTime.UtcNow,
            Status = RentalRequestStatus.Pending
        };
        _context.RentalRequests.Add(rentalRequest);
        await _context.SaveChangesAsync(cancellationToken);
        var rentalRequestWithNav = await _context.RentalRequests
            .Include(rr => rr.Book)
            .Include(rr => rr.Reader)
            .FirstOrDefaultAsync(rr => rr.Id == rentalRequest.Id, cancellationToken);
        return _mapper.Map<RentalRequestDto>(rentalRequestWithNav!);
    }
}