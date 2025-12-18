using AutoMapper;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Profile.Commands.UpdateReaderProfile;
public class UpdateReaderProfileCommandHandler : IRequestHandler<UpdateReaderProfileCommand, ReaderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateReaderProfileCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ReaderDto> Handle(UpdateReaderProfileCommand request, CancellationToken cancellationToken)
    {
        var reader = await _context.Readers
            .FirstOrDefaultAsync(r => r.UserId == request.UserId, cancellationToken);
        if (reader == null)
        {
            throw new InvalidOperationException("Reader profile not found for the current user.");
        }
        reader.Name = request.Name;
        reader.Address = request.Address;
        reader.Phone = request.Phone;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ReaderDto>(reader);
    }
}