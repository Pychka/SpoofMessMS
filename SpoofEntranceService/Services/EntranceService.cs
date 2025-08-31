using AdditionalHelpers;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using SpoofEntranceService.Entities;
using SpoofSettingsService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpoofEntranceService.Services;

public class EntranceService(SpoofSettings.SpoofSettingsClient settingsClient, SesdbContext context) : SpoofEntrance.SpoofEntranceBase
{
    private readonly SpoofSettings.SpoofSettingsClient _spoofSettingsClient = settingsClient;
    private readonly SesdbContext _context = context;
    public override async Task<Tokens> Authentication(AuthenticationRequest request, ServerCallContext context)
    {
        if (!long.TryParse(context.GetHttpContext().User.FindFirst("UserId")?.Value, out long id))
            return new()
            {

            };
        UserInfo? info = null;
        info ??= await _spoofSettingsClient.GetUserInfoAsync(new() { Id = id });
        Tokens? tokens;

        if (info == null || !info.Status)
            tokens = new()
            {
                Status = false,
                Message = info?.Message ?? "Invalid id"
            };
        else
            tokens = new()
            {
                Status = true,
                Access = AccessToken(id),
                Refresh = await RefreshToken(id),
                Message = "Ok"
            };

        return tokens;
    }
    public override Task<Tokens> Authorization(AuthorizationRequest request, ServerCallContext context)
    {
        return base.Authorization(request, context);
    }
    public override async Task<RegResponse> Registration(RegRequest request, ServerCallContext context)
    {
        CreateUserRequest curequest = new()
        {
            Login = request.Name,
            Name = request.Name
        };

        var respose = await _spoofSettingsClient.CreateUserAsync(curequest);
        if (!respose.Status)
            return new()
            {
                Status = false,
                Message = respose.Message
            };

        UserEntry user = new()
        {
            Id = respose.Id ?? 0L,
            Password = request.Password,
            Salt = request.Password
        };

        await _context.UserEntries.AddAsync(user);
        await _context.SaveChangesAsync();

        RegResponse response = new()
        {
            Access = AccessToken(user.Id),
            Refresh = await RefreshToken(user.Id),
            Message = "Ok",
            Id = user.Id,
            Status = true
        };
        return response;
    }
    private async Task<string> RefreshToken(long userId)
    {
        string token = "";
        do
        {
            token = Guid.NewGuid().ToString();
        }
        while (false);
        Token refresh = new()
        {
            Token1 = token,
            ValidTo = DateTime.UtcNow.AddMonths(6),
            UserId = userId,
        };
        await _context.Tokens.AddAsync(refresh);
        await _context.SaveChangesAsync();
        //Добавить в редис
        return token;
    }
    private static string AccessToken(long userId)
    {
        DateTime now = DateTime.UtcNow;
        List<Claim> claims = [new Claim("UserId", userId.ToString())];

        JwtSecurityToken jwt = new(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType).Claims,
            expires: now.Add(TimeSpan.FromMinutes(15)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
