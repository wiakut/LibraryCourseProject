using AutoMapper;
using LibrarySystem.Application.Common.Dtos;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Readers.Queries.GetAllReaders;
public class GetAllReadersPagedQueryHandler : IRequestHandler<GetAllReadersPagedQuery, PagedResultDto<ReaderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllReadersPagedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PagedResultDto<ReaderDto>> Handle(GetAllReadersPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Readers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchLower = request.Search.ToLower();
            query = query.Where(r =>
                r.Name.ToLower().Contains(searchLower) ||
                r.Address.ToLower().Contains(searchLower) ||
                r.Phone.ToLower().Contains(searchLower));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var readers = await query
            .OrderBy(r => r.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        var readerDtos = _mapper.Map<List<ReaderDto>>(readers);
        return new PagedResultDto<ReaderDto>
        {
            Items = readerDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}