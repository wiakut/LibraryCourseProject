using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace LibrarySystem.Infrastructure.Data.Configurations;
public class RentalRequestConfiguration : IEntityTypeConfiguration<RentalRequest>
{
    public void Configure(EntityTypeBuilder<RentalRequest> builder)
    {
        builder.ToTable("RentalRequests");
        builder.HasKey(rr => rr.Id);
        builder.Property(rr => rr.RequestDate)
            .IsRequired();
        builder.Property(rr => rr.Status)
            .HasConversion(new EnumToNumberConverter<RentalRequestStatus, int>())
            .IsRequired();
        builder.Property(rr => rr.DenialReason)
            .HasMaxLength(500);
        builder.HasOne(rr => rr.Book)
            .WithMany(b => b.RentalRequests)
            .HasForeignKey(rr => rr.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(rr => rr.Reader)
            .WithMany()
            .HasForeignKey(rr => rr.ReaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}