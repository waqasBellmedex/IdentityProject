using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;


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
   
        public static IServiceCollection AddGenericRepositories(this IServiceCollection services, Type[] domainTypes)
        {
            var genericRepoInterface = typeof(IRepository<>);
            var genericRepoImplementation = typeof(Repository<>);

            foreach (var domainType in domainTypes)
            {
                var interfaceType = domainType.GetInterfaces().FirstOrDefault();
                if (interfaceType == null)
                    continue;

                var repoInterface = genericRepoInterface.MakeGenericType(domainType);
                var repoImplementation = genericRepoImplementation.MakeGenericType(domainType);
                services.AddScoped(repoInterface, repoImplementation);
            }
            return services;
        }
        public static void ResolveRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IRepository<ApplicationUser>, Repository<ApplicationUser>>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }


    }
}
