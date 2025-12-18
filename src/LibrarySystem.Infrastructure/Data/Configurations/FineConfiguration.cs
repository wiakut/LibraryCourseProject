using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LibrarySystem.Infrastructure.Data.Configurations;
public class FineConfiguration : IEntityTypeConfiguration<Fine>
{
    public void Configure(EntityTypeBuilder<Fine> builder)
    {
        builder.ToTable("Fines");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(f => f.Reason)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(f => f.CreatedDate)
            .IsRequired();
        builder.HasOne(f => f.RentalTransaction)
            .WithMany(rt => rt.Fines)
            .HasForeignKey(f => f.RentalTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}