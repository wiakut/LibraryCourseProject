using AutoMapper;
using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.UpdateReaderCategory;
public class UpdateReaderCategoryCommandHandler : IRequestHandler<UpdateReaderCategoryCommand, ReaderCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateReaderCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderCategoryDto> Handle(UpdateReaderCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.ReaderCategories
            .FirstOrDefaultAsync(rc => rc.Id == request.Id, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Reader category with ID {request.Id} not found.");
        }
        category.Name = request.Name;
        category.DiscountPercentage = request.DiscountPercentage;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ReaderCategoryDto>(category);
    }
}