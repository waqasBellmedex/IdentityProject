using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class DependencyInjectionContext
    {
        public static IServiceCollection RunDatabaseProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(MyDbContext).Assembly.FullName));
            });
         
            var appSettingsSection = configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(appSettingsSection);
            var jwtSettings = appSettingsSection.Get<JwtSettings>();
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

            services.ResolveRepositories();
            services.AddScoped<IAccountService, AccountService>();
            return services;
        }
        public static void ResolveRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ApplicationUser>, Repository<ApplicationUser>>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }


    }
}
