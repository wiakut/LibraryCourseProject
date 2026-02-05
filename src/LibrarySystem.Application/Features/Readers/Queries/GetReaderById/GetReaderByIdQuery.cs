using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Readers.Queries.GetReaderById;
public record GetReaderByIdQuery(Guid Id) : IRequest<ReaderDto?>;