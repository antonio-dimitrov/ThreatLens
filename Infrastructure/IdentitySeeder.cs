using Microsoft.AspNetCore.Identity;
using ThreatLens.Api.Models;

namespace ThreatLens.Api.Infrastructure
{
    public static class IdentitySeeder
    {

        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("IdentitySeeder");

            // 1) the role itself
            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));

            // 2) which user becomes admin (taken from settings, never hardcoded)
            var adminEmail = configuration["Seed:AdminEmail"];

            if (string.IsNullOrWhiteSpace(adminEmail))
                return;

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                logger.LogWarning(
                    "Seed:AdminEmail is set but no user with that email exists yet. Register first, then restart.");
                return;
            }

            if (!await userManager.IsInRoleAsync(user, AppRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, AppRoles.Admin);
                logger.LogInformation("User {UserId} was added to the Admin role.", user.Id);
            }
        }

    }
}
