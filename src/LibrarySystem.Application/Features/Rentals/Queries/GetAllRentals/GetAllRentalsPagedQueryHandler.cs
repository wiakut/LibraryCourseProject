using AutoMapper;
using LibrarySystem.Application.Common.Dtos;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LibrarySystem.Application.Features.Rentals.Queries.GetAllRentals;
public class GetAllRentalsPagedQueryHandler : IRequestHandler<GetAllRentalsPagedQuery, PagedResultDto<RentalTransactionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllRentalsPagedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PagedResultDto<RentalTransactionDto>> Handle(GetAllRentalsPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.RentalTransactions
            .Include(rt => rt.Book)
            .Include(rt => rt.Reader)
            .AsNoTracking()
            .AsQueryable();
        if (request.Status.HasValue)
        {
            query = query.Where(rt => rt.Status == request.Status.Value);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchLower = request.Search.ToLower();
            query = query.Where(rt =>
                rt.Book.Title.ToLower().Contains(searchLower) ||
                rt.Book.Author.ToLower().Contains(searchLower) ||
                rt.Reader.Name.ToLower().Contains(searchLower) ||
                rt.Reader.Phone.ToLower().Contains(searchLower));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var rentals = await query
            .OrderByDescending(rt => rt.IssueDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        var rentalDtos = _mapper.Map<List<RentalTransactionDto>>(rentals);
        return new PagedResultDto<RentalTransactionDto>
        {
            Items = rentalDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}