using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.Infrastructure.Data.Configurations;

public class ReaderCategoryConfiguration : IEntityTypeConfiguration<ReaderCategory>
{
    public void Configure(EntityTypeBuilder<ReaderCategory> builder)
    {
        builder.ToTable("ReaderCategories");

        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rc => rc.DiscountPercentage)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.HasMany(rc => rc.Readers)
            .WithOne(r => r.ReaderCategory)
            .HasForeignKey(r => r.ReaderCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


