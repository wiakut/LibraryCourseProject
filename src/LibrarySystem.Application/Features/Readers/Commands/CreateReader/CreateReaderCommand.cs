using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Readers.Commands.CreateReader;
public record CreateReaderCommand(
    string Name,
    string Address,
    string Phone,
    Guid ReaderCategoryId
) : IRequest<ReaderDto>;