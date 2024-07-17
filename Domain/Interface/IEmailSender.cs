using Domain.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEmailSender
    {
        Task SendCustomerCredentialsEmail(string toEmail, string confirmationLink);
    }
}
