using BackgroundTasks.Interface;
using Domain.Common;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account;
using Domain.Services.Account.Dto;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityProject.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        private readonly IMediator _mediator;
        //private readonly ILogger<AccountService> _logger;
        private readonly IJobQueue<EmailRequest> _emailQueue;
        public AccountController(IAccountService accountService, IMediator mediator, IJobQueue<EmailRequest> emailQueue)
        {
            _accountService = accountService;
            _mediator = mediator;
            _emailQueue = emailQueue;

        }

    [HttpPost(nameof(Register))]
    public async Task<ActionResult<Response>> Register([FromBody] RegistrationRequest request)
        {
            Logger.LogInformation($"{nameof(Register)} method running");
           
            var result = await _accountService.Register(request);
            SignalrService?.SendReload(new SignalRRequestData<object>
            {
                UserId = CurrentUserService.UserId.ToString(),

            });
            return result;
        }


        [HttpGet]
        public  async  Task<ApplicationUser> Get(long id)
        {
            var result = _mediator.Send(id);
            return null;
            //var result = await _accountService.Get(request);
            //return new  ApplicationUser{ };
        }

        [HttpGet]
        [Route("confirm")]
        public async Task<Response> ConfirmEmail(string token, string userId)
        {
            var response = await _accountService.ConfirmEmailAsync(token, userId);
            if (response.IsSuccess)
            {
                return new Response { IsSuccess = true, Message = "Verify Sucessfully" };
            }
            else
            {
               return new Response { IsSuccess = false, Message = "Verification Failed" };
            }
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<Response<AuthenticationResponse>>> Login(AuthenticationRequest request)
        {
            var result = await _accountService.Login(request);
            return result;
        }
        [HttpPost("email")]
        public IActionResult EnqueueEmail([FromBody] EmailRequest job)
        {
            _emailQueue.Enqueue(job);
            return Ok("Email job queued.");
        }

        [HttpPost("Google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenRequest request)
        {
            var validPayLoad = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings()
            {
            Audience = new[] { "65424249423-nr70mimaig8c2rtk33211jukh743rudr.apps.googleusercontent.com" }
            });
            if (validPayLoad == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, validPayLoad.Subject),
                new Claim(ClaimTypes.Email, validPayLoad.Email),
                new Claim(ClaimTypes.Name, validPayLoad.Name)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("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"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
               issuer: "yourapp",
               audience: "yourapp",
               claims: claims,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
