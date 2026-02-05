using AutoMapper;
using LibrarySystem.Application.Features.Books.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Books.Commands.CreateBook;
public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateBookCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            Genre = request.Genre,
            PledgeValue = request.PledgeValue,
            BaseRentalCost = request.BaseRentalCost,
            TotalCount = request.TotalCount,
            AvailableCount = request.TotalCount,
            InRentCount = 0
        };
        _context.Books.Add(book);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BookDto>(book);
    }
}