using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace SpoofSettingsService.Services;

public class SpoofSettingsService(SssdbContext sssdbContext, IConnectionMultiplexer redis) : SpoofSettings.SpoofSettingsBase
{
    private readonly SssdbContext _sssdbContext = sssdbContext;
    private readonly IConnectionMultiplexer _redis = redis;
    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var db = _redis.GetDatabase();
        var uniqueName = await db.StringGetAsync($"uniquename:{request.Login}");
        UniqueName? login;
        if (uniqueName.HasValue)
            login = JsonSerializer.Deserialize<UniqueName>(uniqueName!);
        else
        {
            login = await _sssdbContext.UniqueNames.FirstOrDefaultAsync(x => x.Name == request.Login);
            if (login != null)
                await db.StringSetAsync($"uniquename:{request.Login}", JsonSerializer.Serialize(login), TimeSpan.FromMinutes(10));
        }

        if (login != null && login.IsActive)
            return new()
            {
                Status = false,
                Message = "Login is busy"
            };

        login = new()
        {
            Name = request.Login,
            IsActive = true
        };

        User user = new()
        {
            Name = request.Name,
            UniqueNames = [login]
        };

        await _sssdbContext.Users.AddAsync(user);
        await _sssdbContext.UniqueNames.AddAsync(login);
        await _sssdbContext.SaveChangesAsync();

        await db.StringSetAsync($"user:{user}", JsonSerializer.Serialize(login), TimeSpan.FromMinutes(10));
        return new()
        {
            Id = user.Id,
            Message = "Ok",
            Status = true,
        };
    }

    public override async Task<UserInfo> GetUserInfo(GetUserRequest request, ServerCallContext context)
    {
        var user = await _sssdbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (user == null || user.IsDeleted || !user.SearchMe)
            return new()
            {
                Status = false,
                Message = "User is hidden"
            };
        UserInfo userInfo = new()
        {
            Status = true,
            Name = user.Name,
            WasOnline = user.WasOnline.ToTimestamp()
        };
        if (long.TryParse(context.GetHttpContext().User.FindFirst("UserId")?.Value, out long id))
        {
            userInfo.IsDeleted = user.IsDeleted;
            userInfo.SearchMe = user.SearchMe;
            userInfo.MonthsBeforeDelete = user.MonthsBeforeDelete;
            userInfo.ForwardMessage = user.ForwardMessage;
            userInfo.InviteMe = user.InviteMe;
            userInfo.IsOnline = user.IsOnline;
            userInfo.ShowMe = user.ShowMe;
        }
        return userInfo;
    }
}
