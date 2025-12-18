using AutoMapper;
using LibrarySystem.Application.Common.Dtos;
using LibrarySystem.Application.Features.Books.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Books.Queries.GetAllBooks;
public class GetAllBooksPagedQueryHandler : IRequestHandler<GetAllBooksPagedQuery, PagedResultDto<BookDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllBooksPagedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PagedResultDto<BookDto>> Handle(GetAllBooksPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Books.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchLower = request.Search.ToLower();
            query = query.Where(b =>
                b.Title.ToLower().Contains(searchLower) ||
                b.Author.ToLower().Contains(searchLower) ||
                b.Genre.ToLower().Contains(searchLower));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var books = await query
            .OrderBy(b => b.Title)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        var bookDtos = _mapper.Map<List<BookDto>>(books);
        return new PagedResultDto<BookDto>
        {
            Items = bookDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}