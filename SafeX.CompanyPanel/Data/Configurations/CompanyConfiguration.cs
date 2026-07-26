using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.PasswordHash)
                .IsRequired();

            builder.Property(c => c.Phone)
                .HasMaxLength(30);

            builder.Property(c => c.Address)
                .HasMaxLength(500);

            builder.Property(c => c.Website)
                .HasMaxLength(500);

            builder.Property(c => c.Industry)
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .HasMaxLength(2000);

            builder.Property(c => c.LogoPath)
                .HasMaxLength(500);

            builder.Property(c => c.PasswordResetToken)
                .HasMaxLength(500);

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.HasMany(c => c.CompanyVerifications)
                .WithOne(cv => cv.Company)
                .HasForeignKey(cv => cv.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Jobs)
                .WithOne(j => j.Company)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Hires)
                .WithOne(h => h.Company)
                .HasForeignKey(h => h.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
