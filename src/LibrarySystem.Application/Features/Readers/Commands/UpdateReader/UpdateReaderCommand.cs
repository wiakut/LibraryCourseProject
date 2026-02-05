using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Readers.Commands.UpdateReader;
public record UpdateReaderCommand(
    Guid Id,
    string Name,
    string Address,
    string Phone,
    Guid ReaderCategoryId
) : IRequest<ReaderDto>;