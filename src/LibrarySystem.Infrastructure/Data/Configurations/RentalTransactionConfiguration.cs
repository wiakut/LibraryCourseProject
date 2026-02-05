using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace LibrarySystem.Infrastructure.Data.Configurations;
public class RentalTransactionConfiguration : IEntityTypeConfiguration<RentalTransaction>
{
    public void Configure(EntityTypeBuilder<RentalTransaction> builder)
    {
        builder.ToTable("RentalTransactions");
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.IssueDate)
            .IsRequired();
        builder.Property(rt => rt.ExpectedReturnDate)
            .IsRequired();
        builder.Property(rt => rt.Status)
            .HasConversion(new EnumToNumberConverter<RentalStatus, int>())
            .IsRequired();
        builder.Property(rt => rt.BaseRentalCost)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(rt => rt.FinalRentalCost)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(rt => rt.PledgeAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(rt => rt.RefundAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(rt => rt.FineAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.HasOne(rt => rt.Book)
            .WithMany(b => b.RentalTransactions)
            .HasForeignKey(rt => rt.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(rt => rt.Reader)
            .WithMany(r => r.RentalTransactions)
            .HasForeignKey(rt => rt.ReaderId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(rt => rt.Fines)
            .WithOne(f => f.RentalTransaction)
            .HasForeignKey(f => f.RentalTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}