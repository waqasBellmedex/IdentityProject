namespace IdentityProject.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }   

        public async Task InvokeAsync(HttpContext httpClient)
        {
            try
            {
                await _next(httpClient);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);

            }
        }
        
    }
}
