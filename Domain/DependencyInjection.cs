using AutoMapper;
using Domain.Interface;
using Domain.Modules;
using Domain.Services.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class DependencyInjection
    {

      public static IServiceCollection AddApplication(this IServiceCollection service,IConfiguration configuration)
        {
            var userMappingProfile = ApplicationModule.UserMappingProfile();


            var mappingConfig = new MapperConfiguration(mc => 
            { 
            mc.AddProfile(userMappingProfile);
            });

            var mapper = mappingConfig.CreateMapper();
            service.AddSingleton(mapper);

            service.ResolveServices();
            return service;
        }
        private static void ResolveServices(this IServiceCollection services)
        {
            //services.AddScoped<IAccountService,AccountService>();

        }

    }
}
