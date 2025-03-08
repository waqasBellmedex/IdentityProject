using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Domain.Services.Account
{

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly JwtSettings _jwtSettings;

        public AccountService(IOptions<JwtSettings> jwtSettings,IMapper mapper, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
          _jwtSettings = jwtSettings.Value;
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
            await ValidateUser(request, user);
            var userResponse = await GenerateUserToken(user!);
            MapUser(user, userResponse);

            //if (user != null)
            //{
            //    var userRoles = await _userManager.GetRolesAsync(user);
            //    userResponse.Result.Roles = userRoles.ToList();
            //}
            return userResponse;
        }

        private void MapUser(ApplicationUser user, Response<AuthenticationResponse> userResponse)
        {
            var map = _mapper.Map<AuthenticationResponse>(user);
            var token = userResponse.Result.Token;
            userResponse.Result = map;
            userResponse.Result.Token = token;
        }

        public async  Task<Response<AuthenticationResponse>> GenerateUserToken(ApplicationUser user)
        {
            var userRoles =  await _userManager.GetRolesAsync(user);

            var apiVersion = GetApiVersionForRoles(userRoles);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("ApiVersion",apiVersion)
                    }),
                Expires = DateTime.Now.AddYears(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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

        private string GetApiVersionForRoles(IList<string> roles)
        {
            if (roles.Contains("Admin")) return "2.0";
            if (roles.Contains("User")) return "1.0";
            return "1.0";
        }
        private async Task ValidateUser(AuthenticationRequest request,ApplicationUser? user)
        {
            if(user==null)
             throw new ValidationException($"No user is registered with {request.Username}.");

             var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!result)
                throw new ValidationException($"Invalid Credentials for {request.Username}.");
        }

    
    }
}
