using BackgroundTasks.Interface;
using Domain.Common;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account;
using Domain.Services.Account.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    }
}
