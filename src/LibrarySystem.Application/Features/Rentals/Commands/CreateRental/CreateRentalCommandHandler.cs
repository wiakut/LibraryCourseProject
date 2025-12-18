using AutoMapper;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Pricing;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Rentals.Commands.CreateRental;
public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, RentalTransactionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly PricingStrategyFactory _strategyFactory;
    public CreateRentalCommandHandler(IApplicationDbContext context, IMapper mapper, PricingStrategyFactory strategyFactory)
    {
        _context = context;
        _mapper = mapper;
        _strategyFactory = strategyFactory;
    }
    public async Task<RentalTransactionDto> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.BookId} not found.");
        }
        var reader = await _context.Readers
            .Include(r => r.ReaderCategory)
            .FirstOrDefaultAsync(r => r.Id == request.ReaderId, cancellationToken);
        if (reader == null)
        {
            throw new KeyNotFoundException($"Reader with ID {request.ReaderId} not found.");
        }
        if (book.AvailableCount <= 0)
        {
            throw new InvalidOperationException("Book is not available. No copies available for rental.");
        }
        var issueDate = DateTime.UtcNow;
        if (request.ExpectedReturnDate <= issueDate)
        {
            throw new ArgumentException("Expected return date must be in the future.");
        }
        var pricingStrategy = _strategyFactory.GetPricingStrategy(reader.ReaderCategory);
        var baseRentalCost = pricingStrategy.CalculateRentalCost(book, issueDate, request.ExpectedReturnDate, reader.ReaderCategory);
        var discountPercentage = pricingStrategy.CalculateDiscount(book, reader.ReaderCategory);
        var discountAmount = baseRentalCost * (discountPercentage / 100);
        var finalRentalCost = baseRentalCost - discountAmount;
        var rental = new RentalTransaction
        {
            Id = Guid.NewGuid(),
            BookId = request.BookId,
            ReaderId = request.ReaderId,
            IssueDate = issueDate,
            ExpectedReturnDate = request.ExpectedReturnDate,
            Status = RentalStatus.Active,
            BaseRentalCost = baseRentalCost,
            FinalRentalCost = finalRentalCost,
            PledgeAmount = book.PledgeValue,
            RefundAmount = 0,
            FineAmount = 0
        };
        _context.RentalTransactions.Add(rental);
        book.AvailableCount--;
        book.InRentCount++;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RentalTransactionDto>(rental);
    }
}