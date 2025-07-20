using BackgroundTasks;
using Database;
using Domain;
using Domain.Model;
using EmailConfiguration;
using FluentAssertions.Common;
using IdentityProject;
using IdentityProject.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var domainTypes = new[]
{
    typeof(ApplicationUser)
};

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtention();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
DependencyInjection.AddApplication(builder.Services, builder.Configuration);
DependencyInjectionContext.RunDatabaseProjectServices(builder.Services, builder.Configuration);
EmailDependency.ResolveEmailDependency(builder.Services, builder.Configuration);
BackgroundTaskDependency.RunBackGroundTaskServices(builder.Services,builder.Configuration);
DependencyInjectionContext.AddGenericRepositories(builder.Services, domainTypes);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
    {
        dbContext.Database.Migrate();
    }
}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwaggerExtension(builder.Configuration);

app.UseHttpsRedirection();
app.UseCustomMiddlewears();
app.UseAuthorization();
app.UseCors(options => options
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin());
app.MapControllers();

app.Run();
