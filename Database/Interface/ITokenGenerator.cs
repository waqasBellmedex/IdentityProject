using Domain.Common;
using Domain.Model;
using Domain.Services.Account.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Interface
{
    public interface ITokenGenerator
    {
        Response<AuthenticationResponse> GenerateUserToken(ApplicationUser applicationUser);
    }
}
