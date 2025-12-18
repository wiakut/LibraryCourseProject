using AutoMapper;
using LibrarySystem.Application.Features.Books.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
namespace LibrarySystem.Application.Features.Books.Queries.GetBookById;
public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetBookByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }
}