using Database;
using Domain;
using Domain.Model;
using EmailConfiguration;
using FluentAssertions.Common;
using IdentityProject.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerExtention();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
DependencyInjection.AddApplication(builder.Services, builder.Configuration);
DependencyInjectionContext.RunDatabaseProjectServices(builder.Services, builder.Configuration);
EmailDependency.ResolveEmailDependency(builder.Services, builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwaggerExtension(builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(options => options
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin());
app.MapControllers();

app.Run();
