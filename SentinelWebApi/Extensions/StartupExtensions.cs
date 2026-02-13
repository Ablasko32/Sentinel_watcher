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

            try
            {
                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to seed roles");
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
                try
                {
                    await dbContext.Database.MigrateAsync();
                }
                catch (Exception)
                {
                    throw new Exception("Failed to migrate database");
                }
            }

            return app;
        }

        public static async Task<IApplicationBuilder> SeedAdminUserAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var admin = configuration.GetSection("AdminUser");
            if (!admin.Exists())
            {
                throw new Exception("AdminUser configuration section is missing.");
            }
            var adminEmail = admin.GetValue<string>("Email");
            var adminPassword = admin.GetValue<string>("Password");
            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new Exception("AdminUser Email or Password is missing in configuration.");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Role.Admin.ToString());
                }
                else
                {
                    throw new Exception("Failed to create Admin user");
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
                options.Cookie.Name = "SentinelAuthCookie";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
                if (builder.Environment.IsDevelopment())
                {
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                }
                else
                {
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                }
            });

            return builder;
        }
    }
}