using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Genre)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.PledgeValue)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.BaseRentalCost)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.TotalCount)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(b => b.AvailableCount)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(b => b.InRentCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasMany(b => b.RentalTransactions)
            .WithOne(rt => rt.Book)
            .HasForeignKey(rt => rt.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.RentalRequests)
            .WithOne(rr => rr.Book)
            .HasForeignKey(rr => rr.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


