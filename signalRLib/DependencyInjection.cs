using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace signalRLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSignalRService(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddSignalRCore();

            return service;
        }

    }
}
