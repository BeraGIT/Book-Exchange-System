using DAL.Context;
using ENTITIES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DependencyResolvers
{
    public static class IdentityExtensionService
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<string>>(x =>
            {
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequiredLength = 3;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;

                // Kullanıcı kilitleme ayarları
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                x.Lockout.MaxFailedAccessAttempts = 5;
                x.Lockout.AllowedForNewUsers = true;

                // Kullanıcı ayarları
                x.User.RequireUniqueEmail = false;
                x.SignIn.RequireConfirmedEmail = false;
                x.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<MyContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
