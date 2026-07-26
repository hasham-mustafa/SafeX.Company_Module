using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data.Configurations
{
    public class CompanyVerificationConfiguration : IEntityTypeConfiguration<CompanyVerification>
    {
        public void Configure(EntityTypeBuilder<CompanyVerification> builder)
        {
            builder.ToTable("CompanyVerifications");

            builder.HasKey(cv => cv.Id);

            builder.Property(cv => cv.DocumentType)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(cv => cv.DocumentPath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(cv => cv.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(cv => cv.ReviewedBy)
                .HasMaxLength(200);

            builder.Property(cv => cv.Remarks)
                .HasMaxLength(1000);
        }
    }
}
