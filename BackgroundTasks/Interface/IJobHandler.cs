using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks.Interface
{
    public interface IJobHandler<T>
    {
        Task HandleAsync(T job, CancellationToken cancellationToken);
    }
}
