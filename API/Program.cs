using API.Extensions;
using API.Helpers;
using API.Middleware;
using Insfrastructure.Data;
using Insfrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region Configure Database Connection
var productConnString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(c => c.UseSqlite(productConnString));

var identityConnString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlite(identityConnString));
#endregion

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));

#region Configure Identity (JWT and Cookie)
builder.Services.AddIdentityServices(builder.Configuration);
#endregion

#region Configure Appicaltion Services
builder.Services.AddApplicationServices();
#endregion

#region Configure Cookie Service
builder.Services.AddCookieServices();
#endregion

// Cross-origin resource sharing
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});

var app = builder.Build();
// Apply migrations and seed data
await app.MigrateAndSeedDatabase();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();