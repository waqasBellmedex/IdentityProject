﻿using Domain.Common;
using Domain.Services.Account.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAccountService
    {
        Task<Response> Register(RegistrationRequest request);
        Task<Response> ConfirmEmailAsync(string token, string userId);
        Task<Response<AuthenticationResponse>> Login(AuthenticationRequest request);
        //Task<Response> Get(GetUserRequestDto request);
    }
}
