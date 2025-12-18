using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Profile.Queries.GetReaderByUserId;
public record GetReaderByUserIdQuery(string UserId) : IRequest<ReaderDto?>;