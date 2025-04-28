using BackgroundTasks.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks.Service
{
    public class GenericJobService<T> : BackgroundService
    {

        private readonly IJobQueue<T> _queue;
        private readonly IJobHandler<T> _handler;
        private readonly ILogger<GenericJobService<T>> _logger;
        public GenericJobService(IJobQueue<T> queue, IJobHandler<T> handler, ILogger<GenericJobService<T>> logger)
        {
            _queue = queue;
            _handler = handler;
            _logger = logger;
        }



        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await foreach (var job in _queue.Reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    await _handler.HandleAsync(job, cancellationToken);
                }
                catch(Exception ex)  
                {
                    _logger.LogError(ex, $"job processing failed for {typeof(T).Name}");
                }
            }
        }
    }
}
