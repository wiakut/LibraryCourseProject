using AutoMapper;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId;
public class GetReaderByUserIdQueryHandler : IRequestHandler<GetReaderByUserIdQuery, ReaderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetReaderByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderDto?> Handle(GetReaderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var reader = await _context.Readers
            .Include(r => r.ReaderCategory)
            .FirstOrDefaultAsync(r => r.UserId == request.UserId, cancellationToken);
        return reader == null ? null : _mapper.Map<ReaderDto>(reader);
    }
}