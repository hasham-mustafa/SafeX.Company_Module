using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data.Configurations;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies => Set<Company>();
        public DbSet<CompanyVerification> CompanyVerifications => Set<CompanyVerification>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Applicant> Applicants => Set<Applicant>();
        public DbSet<Hire> Hires => Set<Hire>();
        public DbSet<Admin> Admins => Set<Admin>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyVerificationConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicantConfiguration());
            modelBuilder.ApplyConfiguration(new HireConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
        }
    }
}
