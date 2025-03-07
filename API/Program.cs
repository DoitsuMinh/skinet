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
builder.Services.AddDbContext<StoreContext>(
    options => options.UseSqlite(productConnString,
    x => x.MigrationsAssembly("Infrastructure.DataMigrations")
    ));

var identityConnString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseSqlite(identityConnString,
        x => x.MigrationsAssembly("Infrastructure.IdentityMigrations")));
#endregion

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var connString = builder.Configuration.GetConnectionString("Redis")
                     ?? throw new Exception("Cannot get redis connection string");
    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
});

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
        policy.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
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

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();