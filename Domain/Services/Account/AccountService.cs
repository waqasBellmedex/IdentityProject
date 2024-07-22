using AutoMapper;
using Domain.Common;
using Domain.Interface;
using Domain.Model;
using Domain.Options;
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
        private readonly IEmailSender _emailSender;
        public AccountService(IMapper mapper, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
          _mapper = mapper;
          _userManager = userManager;
          _emailSender = emailSender;
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
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return new Response { IsSuccess = false, Message = "User is null or missing email" };
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"https://yourapp.com/confirm?token={Uri.EscapeDataString(token)}&userId={user.Id}";

            await _emailSender.SendCustomerCredentialsEmail(user.Email, confirmationLink);

            return new Response { IsSuccess = true, Message = "SaveSucessfully", Errors = null! };

        }
        public async Task<Response> ConfirmEmailAsync(string token, string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                return new Response { IsSuccess = false, Message = "Invalid email confirmation request." };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response { IsSuccess = false, Message = "User not found." };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.IsActive = true; // Assuming you have an IsActive property
                await _userManager.UpdateAsync(user);
                return new Response { IsSuccess = true, Message = "Email confirmed successfully." };
            }
            else
            {
                return new Response { IsSuccess = false, Message = "Error confirming email." };
            }
        }
  
}
}
