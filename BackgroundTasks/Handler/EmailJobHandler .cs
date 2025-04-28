using BackgroundTasks.Interface;
using Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks.Handler
{
    public class EmailJobHandler : IJobHandler<EmailRequest>
    {
        private readonly ILogger<EmailJobHandler> _logger;

        public EmailJobHandler(ILogger<EmailJobHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(EmailRequest job, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[Email] Sending to {job.To}: {job.Subject}");
            await Task.Delay(1000); // Simulate delay
        }
    }
}
