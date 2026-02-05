using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LibrarySystem.Infrastructure.Data.Configurations;
public class ReaderConfiguration : IEntityTypeConfiguration<Reader>
{
    public void Configure(EntityTypeBuilder<Reader> builder)
    {
        builder.ToTable("Readers");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(r => r.Address)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(r => r.Phone)
            .IsRequired()
            .HasMaxLength(20);
        builder.Property(r => r.UserId)
            .HasMaxLength(450);
        builder.HasOne(r => r.ReaderCategory)
            .WithMany(rc => rc.Readers)
            .HasForeignKey(r => r.ReaderCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(r => r.RentalTransactions)
            .WithOne(rt => rt.Reader)
            .HasForeignKey(rt => rt.ReaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}