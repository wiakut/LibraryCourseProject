using AutoMapper;
using LibrarySystem.Application.Features.Books.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Books.Commands.UpdateBook;
public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateBookCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.Id} not found.");
        }
        book.Title = request.Title;
        book.Author = request.Author;
        book.Genre = request.Genre;
        book.PledgeValue = request.PledgeValue;
        book.BaseRentalCost = request.BaseRentalCost;
        if (request.TotalCount < book.InRentCount)
        {
            throw new InvalidOperationException($"Total count cannot be less than the number of copies currently in rent ({book.InRentCount}).");
        }
        book.TotalCount = request.TotalCount;
        book.AvailableCount = book.TotalCount - book.InRentCount;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BookDto>(book);
    }
}