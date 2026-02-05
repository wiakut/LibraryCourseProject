using AutoMapper;
using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.ReaderCategories.Queries.GetAllReaderCategories;
public class GetAllReaderCategoriesQueryHandler : IRequestHandler<GetAllReaderCategoriesQuery, IEnumerable<ReaderCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllReaderCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ReaderCategoryDto>> Handle(GetAllReaderCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.ReaderCategories
            .OrderBy(rc => rc.Name)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ReaderCategoryDto>>(categories);
    }
}