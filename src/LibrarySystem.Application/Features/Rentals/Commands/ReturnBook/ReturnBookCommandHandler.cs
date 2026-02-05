using AutoMapper;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Pricing;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Rentals.Commands.ReturnBook;
public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, RentalTransactionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly PricingStrategyFactory _strategyFactory;
    public ReturnBookCommandHandler(IApplicationDbContext context, IMapper mapper, PricingStrategyFactory strategyFactory)
    {
        _context = context;
        _mapper = mapper;
        _strategyFactory = strategyFactory;
    }
    public async Task<RentalTransactionDto> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var rental = await _context.RentalTransactions
            .Include(rt => rt.Book)
            .Include(rt => rt.Reader)
                .ThenInclude(r => r.ReaderCategory)
            .FirstOrDefaultAsync(rt => rt.Id == request.RentalTransactionId, cancellationToken);
        if (rental == null)
        {
            throw new KeyNotFoundException($"Rental transaction with ID {request.RentalTransactionId} not found.");
        }
        if (rental.Status != RentalStatus.Active)
        {
            throw new InvalidOperationException("Only active rentals can be returned.");
        }
        if (request.ActualReturnDate < rental.IssueDate)
        {
            throw new ArgumentException("Actual return date cannot be before issue date.");
        }
        var fineStrategy = _strategyFactory.GetFineStrategy(rental.Reader.ReaderCategory);
        var totalFine = fineStrategy.CalculateFine(rental, request.ActualReturnDate, request.DamageCost, rental.Reader.ReaderCategory);
        var refundAmount = rental.PledgeAmount - totalFine;
        rental.ActualReturnDate = request.ActualReturnDate;
        rental.Status = RentalStatus.Returned;
        rental.FineAmount = totalFine;
        rental.RefundAmount = refundAmount;
        rental.Book.AvailableCount++;
        rental.Book.InRentCount--;
        if (totalFine > 0)
        {
            var fine = new Domain.Entities.Fine
            {
                Id = Guid.NewGuid(),
                RentalTransactionId = rental.Id,
                Amount = totalFine,
                Reason = request.DamageCost > 0 ? $"Overdue fine + Damage cost (${request.DamageCost})" : "Overdue fine",
                CreatedDate = DateTime.UtcNow
            };
            _context.Fines.Add(fine);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RentalTransactionDto>(rental);
    }
}