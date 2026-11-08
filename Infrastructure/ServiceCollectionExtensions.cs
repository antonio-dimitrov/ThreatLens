using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreatLens.Api.Data;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        // Our own services, the database and Identity
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient<VirusTotalService>(client =>
            {
                client.BaseAddress = new Uri("https://www.virustotal.com/api/v3/");
                client.Timeout = TimeSpan.FromSeconds(15);   // the default is 100 seconds
            });

            services.AddScoped<ScanService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddDbContext<ThreatLensContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));

            services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()   // NEW: enables roles (RoleManager, GetRolesAsync, AddToRoleAsync)
                .AddEntityFrameworkStores<ThreatLensContext>();

            return services;
        }

        // Exception handler + ProblemDetails (the middleware itself is app.UseExceptionHandler())
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}

