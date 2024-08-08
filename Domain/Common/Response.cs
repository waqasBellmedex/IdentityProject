using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{

    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Result { get; set; }
    }
    public class Response
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
    }
}
