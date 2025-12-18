using AutoMapper;
using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using MediatR;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.CreateReaderCategory;
public class CreateReaderCategoryCommandHandler : IRequestHandler<CreateReaderCategoryCommand, ReaderCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateReaderCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderCategoryDto> Handle(CreateReaderCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new ReaderCategory
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DiscountPercentage = request.DiscountPercentage
        };
        _context.ReaderCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ReaderCategoryDto>(category);
    }
}