using BackgroundTasks.Handler;
using BackgroundTasks.Interface;
using BackgroundTasks.Service;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks
{
    public static class BackgroundTaskDependency
    {
        public static IServiceCollection RunBackGroundTaskServices(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddSingleton<IJobHandler<EmailRequest>, EmailJobHandler>();
            service.AddSingleton(typeof(IJobQueue<>), typeof(JobQueue<>));
            service.AddHostedService<GenericJobService<EmailRequest>>();
            return service;
        }
    }
}
