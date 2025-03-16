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
    public TgBotRepository(IGetCongr getCongr, IGetTgId getTgId, IGetTodayBDayFriends getTodayBDayFriends)
    {
        _getCongr = getCongr;
        _getTgId = getTgId;
        _getTodayBDayFriends = getTodayBDayFriends;
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
}