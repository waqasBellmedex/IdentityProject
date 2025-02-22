using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class SignalRRequest
    {
        public string UserId { get; set; } = string.Empty;

    }
    public class SignalRRequestData<T> : SignalRRequest
    {
        public List<string> Signals { get; set; }
        public string ConnnectionId { get; set; }
        public T? Data { get; set; }
    }
}
