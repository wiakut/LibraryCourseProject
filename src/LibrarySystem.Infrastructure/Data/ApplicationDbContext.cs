using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Reader> Readers => Set<Reader>();
    public DbSet<ReaderCategory> ReaderCategories => Set<ReaderCategory>();
    public DbSet<RentalTransaction> RentalTransactions => Set<RentalTransaction>();
    public DbSet<Fine> Fines => Set<Fine>();
    public DbSet<RentalRequest> RentalRequests => Set<RentalRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}


