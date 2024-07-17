
using Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EmailConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ResolveEmailDependency(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}