using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SentinelCore.DAL;
using SentinelCore.DAL.Data.Models;
using static SentinelCore.DAL.Data.Models.AppRoles;

namespace SentinelWebApi.Extensions
{
    public static class StartupExtensions
    {
        public static async Task<IApplicationBuilder> SeedRolesAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in new[] { Role.Admin, Role.User })
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            return app;
        }

        public static async Task<IApplicationBuilder> MigrateDbAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<SentinelContext>();

            var migrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (migrations.Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            return app;
        }

        public static async Task<IApplicationBuilder> SeedAdminUserAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var adminEmail = "admin@sentinel.com";
            var adminPassword = "SentinelAdmin";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Role.Admin.ToString());
                }
            }

            return app;
        }

        public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
        {
            //Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<SentinelContext>()
            .AddDefaultTokenProviders();

            // Cookie settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "SentinelCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
            });

            return builder;
        }
    }
}