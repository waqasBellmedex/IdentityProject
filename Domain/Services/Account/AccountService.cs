using AutoMapper;
using Domain.Common;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Account
{

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountService(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
          _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<Response> Register(RegistrationRequest request)
        {
            var user = _mapper.Map<ApplicationUser>(request);
            user.UserName = request.FullName;
            user.SecurityStamp = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result == null)
            {
                return new Response { IsSuccess = false, Message = "can not create user" };
            }

            return new Response { IsSuccess = true, Message = "SaveSucessfully", Errors = null! };

        }
    }
}
