using IdentityProject.Middlewares;

namespace IdentityProject
{
    public static class ApiExtentions
    {

        public static void UseCustomMiddlewears(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
