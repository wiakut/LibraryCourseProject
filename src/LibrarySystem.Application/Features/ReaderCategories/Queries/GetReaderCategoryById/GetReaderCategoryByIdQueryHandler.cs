using AutoMapper;
using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.ReaderCategories.Queries.GetReaderCategoryById;
public class GetReaderCategoryByIdQueryHandler : IRequestHandler<GetReaderCategoryByIdQuery, ReaderCategoryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetReaderCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderCategoryDto?> Handle(GetReaderCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.ReaderCategories
            .FirstOrDefaultAsync(rc => rc.Id == request.Id, cancellationToken);
        if (category == null)
        {
            return null;
        }
        return _mapper.Map<ReaderCategoryDto>(category);
    }
}