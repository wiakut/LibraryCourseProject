using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Profile.Commands.UpdateReaderProfile;
public record UpdateReaderProfileCommand(
    string UserId,
    string Name,
    string Address,
    string Phone
) : IRequest<ReaderDto>;