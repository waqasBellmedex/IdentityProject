using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Account.Dto
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
