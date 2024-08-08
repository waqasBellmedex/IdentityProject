using Database.Interface;
using Domain.Common;
using Domain.Entities;
using Domain.Model;
using Domain.Services.Account.Dto;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repository
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        public TokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }


        public Response<AuthenticationResponse> GenerateUserToken(ApplicationUser applicationUser)
        {

            return null;
        }
    }
}
