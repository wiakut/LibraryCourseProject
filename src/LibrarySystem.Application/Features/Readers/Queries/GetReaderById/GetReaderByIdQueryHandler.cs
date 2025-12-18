using AutoMapper;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Readers.Queries.GetReaderById;
public class GetReaderByIdQueryHandler : IRequestHandler<GetReaderByIdQuery, ReaderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetReaderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderDto?> Handle(GetReaderByIdQuery request, CancellationToken cancellationToken)
    {
        var reader = await _context.Readers
            .Include(r => r.ReaderCategory)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        return reader == null ? null : _mapper.Map<ReaderDto>(reader);
    }
}