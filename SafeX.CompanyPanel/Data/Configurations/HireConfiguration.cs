using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data.Configurations
{
    public class HireConfiguration : IEntityTypeConfiguration<Hire>
    {
        public void Configure(EntityTypeBuilder<Hire> builder)
        {
            builder.ToTable("Hires");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.OfferLetterPath)
                .HasMaxLength(1000);

            builder.Property(h => h.ContractPath)
                .HasMaxLength(1000);

            builder.Property(h => h.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(h => h.Remarks)
                .HasMaxLength(2000);
        }
    }
}
