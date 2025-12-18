using MediatR;
namespace LibrarySystem.Application.Features.Readers.Commands.DeleteReader;
public record DeleteReaderCommand(Guid Id) : IRequest<Unit>;