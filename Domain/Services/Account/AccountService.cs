using AutoMapper;
using Domain.Common;
using Domain.Interface;
using Domain.Model;
using Domain.Options;
using Domain.Services.Account.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
 
        public async Task<Response<AuthenticationResponse>> Login(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if(user!=null)
            {
                await ValidateUser(request, user);
            }
            return null;
        }


        private async Task ValidateUser(AuthenticationRequest request,ApplicationUser? user)
        {
            if(user==null)
             throw new ValidationException($"No user is registered with {request.Username}.");

             var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!result)
                throw new ValidationException($"Invalid Credentials for {request.Username}.");
        }

        private async Task<Response<AuthenticationResponse>> GenerateUserToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                Expires = DateTime.Now.AddYears(5),
                SigningCredentials =
                   new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Response<AuthenticationResponse>
            {
                IsSuccess = true,
                Message = "User Authenticated successfully",
                Result = new AuthenticationResponse
                {
                    Username = user.UserName,
                    Token = tokenHandler.WriteToken(token)
                }
            };
        }


    }
}
