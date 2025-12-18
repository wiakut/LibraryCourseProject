using AutoMapper;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Readers.Commands.UpdateReader;
public class UpdateReaderCommandHandler : IRequestHandler<UpdateReaderCommand, ReaderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateReaderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderDto?> Handle(UpdateReaderCommand request, CancellationToken cancellationToken)
    {
        var reader = await _context.Readers
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (reader == null)
        {
            return null;
        }
        reader.Name = request.Name;
        reader.Address = request.Address;
        reader.Phone = request.Phone;
        reader.ReaderCategoryId = request.ReaderCategoryId;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ReaderDto>(reader);
    }
}