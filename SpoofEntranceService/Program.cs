using AdditionalHelpers;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Entities;
using SpoofEntranceService.Services;
using static SpoofSettingsService.SpoofSettings;

namespace SpoofEntranceService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddGrpc();

        builder.Services.AddDbContext<SesdbContext>(s => s.UseSqlServer("Server=.;Database=AuthDB;TrustServerCertification=True;Trusted_Conection=True"));

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

        builder.Services.AddSingleton(provider =>
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7232", new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
            return new SpoofSettingsClient(channel);
        });
        builder.Services.AddScoped<EntranceService>();
        var app = builder.Build();
        app.MapGrpcService<EntranceService>();
        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();

        app.Run();
    }
}
