using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(a => a.Email)
                .IsUnique();

            builder.Property(a => a.PasswordHash)
                .IsRequired();

            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();

            builder.Property(a => a.LastLoginAt)
                .IsRequired(false);
        }
    }
}