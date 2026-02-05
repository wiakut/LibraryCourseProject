using AutoMapper;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using MediatR;
namespace LibrarySystem.Application.Features.Readers.Commands.CreateReader;
public class CreateReaderCommandHandler : IRequestHandler<CreateReaderCommand, ReaderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateReaderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderDto> Handle(CreateReaderCommand request, CancellationToken cancellationToken)
    {
        var reader = new Reader
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            ReaderCategoryId = request.ReaderCategoryId
        };
        _context.Readers.Add(reader);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ReaderDto>(reader);
    }
}