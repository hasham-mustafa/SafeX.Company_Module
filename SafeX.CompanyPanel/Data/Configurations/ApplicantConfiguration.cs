using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data.Configurations
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.ToTable("Applicants");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Phone)
                .HasMaxLength(30);

            builder.Property(a => a.ProfilePicture)
                .HasMaxLength(500);

            builder.Property(a => a.University)
                .HasMaxLength(200);

            builder.Property(a => a.Skills)
                .HasMaxLength(2000);

            builder.Property(a => a.ResumePath)
                .HasMaxLength(1000);

            builder.Property(a => a.CoverLetter)
                .HasMaxLength(5000);

            builder.Property(a => a.Proposal)
                .HasMaxLength(5000);

            builder.Property(a => a.PortfolioUrl)
                .HasMaxLength(500);

            builder.Property(a => a.LinkedInUrl)
                .HasMaxLength(500);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(a => a.Hire)
                .WithOne(h => h.Applicant)
                .HasForeignKey<Hire>(h => h.ApplicantId);
        }
    }
}
