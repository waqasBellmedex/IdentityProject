using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        private ILogger<T>? _logger;
        private ICurrentUserService? _currentUserService;
        protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>()!;
        protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetService<ICurrentUserService>()!;

    }
}
