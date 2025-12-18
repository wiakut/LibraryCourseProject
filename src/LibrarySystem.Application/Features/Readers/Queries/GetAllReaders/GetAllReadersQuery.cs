using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Readers.Queries.GetAllReaders;
public record GetAllReadersQuery() : IRequest<List<ReaderDto>>;