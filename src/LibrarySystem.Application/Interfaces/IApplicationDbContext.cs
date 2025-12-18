using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Book> Books { get; }
    DbSet<Reader> Readers { get; }
    DbSet<ReaderCategory> ReaderCategories { get; }
    DbSet<RentalTransaction> RentalTransactions { get; }
    DbSet<Fine> Fines { get; }
    DbSet<RentalRequest> RentalRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


