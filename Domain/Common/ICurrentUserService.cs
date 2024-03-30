using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface  ICurrentUserService
    {
        public int UserId { get; }
        bool IsAuthenticated { get; }
    }
}
