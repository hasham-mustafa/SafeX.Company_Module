using SafeX.CompanyPanel.Repositories.Implementations;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Implementations;
using SafeX.CompanyPanel.Services.Interfaces;

namespace SafeX.CompanyPanel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IHireRepository, HireRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IHireService, HireService>();
            services.AddScoped<IFileService, FileService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
