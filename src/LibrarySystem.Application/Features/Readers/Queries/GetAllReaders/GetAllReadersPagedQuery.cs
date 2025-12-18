using LibrarySystem.Application.Common.Dtos;
using LibrarySystem.Application.Features.Readers.Dtos;
using MediatR;
namespace LibrarySystem.Application.Features.Readers.Queries.GetAllReaders;
public record GetAllReadersPagedQuery(
    string? Search = null,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResultDto<ReaderDto>>;