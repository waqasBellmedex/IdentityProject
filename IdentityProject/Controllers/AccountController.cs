using Domain.Common;
using Domain.Interface;
using Domain.Model;
using Domain.Services.Account;
using Domain.Services.Account.Dto;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        //private readonly ILogger<AccountService> _logger;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
          
        }

    [HttpPost(nameof(Register))]
    public async Task<ActionResult<Response>> Register([FromBody] RegistrationRequest request)
        {
            Logger.LogInformation($"{nameof(Register)} method running");
            var result = await _accountService.Register(request);
            return result;
        }


        [HttpGet (nameof(Get))]
        public  async  Task<ActionResult<ApplicationUser>> Get(GetUserRequestDto request )
        {
            //var result = await _accountService.Get(request);
            return new  ApplicationUser{ };
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

    }
}
