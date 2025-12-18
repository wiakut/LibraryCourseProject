using AutoMapper;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Readers.Queries.GetAllReaders;
public class GetAllReadersQueryHandler : IRequestHandler<GetAllReadersQuery, List<ReaderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllReadersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<ReaderDto>> Handle(GetAllReadersQuery request, CancellationToken cancellationToken)
    {
        var readers = await _context.Readers.ToListAsync(cancellationToken);
        return _mapper.Map<List<ReaderDto>>(readers);
    }
}