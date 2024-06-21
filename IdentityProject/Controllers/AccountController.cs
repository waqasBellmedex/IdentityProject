using Domain.Common;
using Domain.Interface;
using Domain.Services.Account;
using Domain.Services.Account.Dto;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountService> _logger;
        public AccountController(IAccountService accountService,ILogger<AccountService> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

    [HttpPost(nameof(Register))]
    public async Task<ActionResult<Response>> Register([FromBody] RegistrationRequest request)
        {
            _logger.LogInformation($"{nameof(Register)} method running");
            var result = await _accountService.Register(request);
            return result;
        }
    }
}
