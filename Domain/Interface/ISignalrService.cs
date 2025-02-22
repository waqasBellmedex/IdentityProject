using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ISignalrService
    {
        public Task SendReload(SignalRRequestData<object> request);
    }
}
