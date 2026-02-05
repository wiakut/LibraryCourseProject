using AutoMapper;
using LibrarySystem.Application.Features.Books.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Books.Queries.GetAllBooks;
public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _context.Books.ToListAsync(cancellationToken);
        return _mapper.Map<List<BookDto>>(books);
    }
}