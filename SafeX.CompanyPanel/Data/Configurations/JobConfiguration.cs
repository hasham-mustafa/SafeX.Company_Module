using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs");

            builder.HasKey(j => j.Id);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(10000);

            builder.Property(j => j.Location)
                .HasMaxLength(500);

            builder.Property(j => j.EmploymentType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(j => j.Category)
                .HasMaxLength(200);

            builder.Property(j => j.Currency)
                .HasMaxLength(10);

            builder.Property(j => j.SkillsRequired)
                .HasMaxLength(2000);

            builder.Property(j => j.ExperienceLevel)
                .HasMaxLength(100);

            builder.Property(j => j.Duration)
                .HasMaxLength(100);

            builder.Property(j => j.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasMany(j => j.Applicants)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.Hires)
                .WithOne(h => h.Job)
                .HasForeignKey(h => h.JobId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
