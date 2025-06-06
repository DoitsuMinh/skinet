using API.Extensions;
using API.Helpers;
using API.Middleware;
using API.SignalR;
using Core.Enitities.Identity;
using Insfrastructure.Data;
using Insfrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region Configure Database Connection
builder.Services.GetSecretConfigureServices(builder.Configuration);
#endregion

builder.Services.AddAutoMapper(typeof(MappingProfiles));

#region Configure Identity
builder.Services.AddIdentityServices();
#endregion

builder.Services.AddJWTAuthenticationServices(builder.Configuration);

#region Configure Application Services
builder.Services.AddApplicationServices();
#endregion

// Cross-origin resource sharing
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("https://localhost:4200", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();
// Apply migrations and seed data
await app.MigrateAndSeedDatabase();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseRouting();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>(); // api/login
app.MapHub<NotificationHub>("/hub/notifications");
app.MapFallbackToController("Index", "Fallback");

app.Run();