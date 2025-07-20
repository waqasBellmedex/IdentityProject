using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Account.Dto
{
    public class GoogleTokenRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
}
