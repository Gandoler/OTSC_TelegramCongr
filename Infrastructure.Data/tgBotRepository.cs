using Domain.DTO.TG;
using Domain.Interfaces;
using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Data;

public class TgBotRepository : ITgBotRepository
{
    private readonly IGetCongr _getCongr;
    private readonly IGetTgId _getTgId;
    private readonly IGetTodayBDayFriends _getTodayBDayFriends;
    private readonly IGetUsername _getUsername;
    public TgBotRepository(IGetCongr getCongr, IGetTgId getTgId, IGetTodayBDayFriends getTodayBDayFriends, 
        IGetUsername getUsername)
    {
        _getCongr = getCongr;
        _getTgId = getTgId;
        _getTodayBDayFriends = getTodayBDayFriends;
        _getUsername = getUsername;
    }

    public Task<string?> GetCongratulationAsync(PozdrikIdDto pozdrikId)
    {
        return _getCongr.GetCongratulationAsync(pozdrikId);
    }

    public Task<TgIdDto?> GetTgId(AppIdDto appId)
    {
        return _getTgId.GetTgId(appId);
    }

    public Task<List<FriendDto>> GetTodayBDayFriendsAsync()
    {
        return _getTodayBDayFriends.GetTodayBDayFriendsAsync();
    }

    public Task<string?> GetUsernameAsync(PozdrikIdDto pozdrikId)
    {
        return _getUsername.GetUsernameAsync(pozdrikId);
    }
}