using AdditionalHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace SpoofSettingsService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<SssdbContext>(s => s.UseSqlServer("Server=.;Database=SSSDB;TrustServerCertification=True;Trusted_Conection=True"));

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis:6379"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
            };
        });

        builder.Services.AddScoped<Services.SpoofSettingsService>();


        var app = builder.Build();

        app.MapGrpcService<Services.SpoofSettingsService>();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();

        app.Run();
    }
}
