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
    private readonly IGetPozdrikId _getPozdrikId;
    public TgBotRepository(IGetCongr getCongr, IGetTgId getTgId, IGetTodayBDayFriends getTodayBDayFriends, 
        IGetPozdrikId getPozdrikId)
    {
        _getCongr = getCongr;
        _getTgId = getTgId;
        _getTodayBDayFriends = getTodayBDayFriends;
        _getPozdrikId = getPozdrikId;
    }

    public Task<string?> GetCongratulationAsync(PozdrikIdDto pozdrikId)
    {
        return _getCongr.GetCongratulationAsync(pozdrikId);
    }

    public async Task<TgIdDto?> GetTgIdAsync(AppIdDto appId)
    {
        return await _getTgId.GetTgIdAsync(appId);
    }

    public async Task<List<FriendDto>> GetTodayBDayFriendsAsync()
    {
        return await _getTodayBDayFriends.GetTodayBDayFriendsAsync();
    }

    public async Task<PozdrikIdDto?> GetPozdrikIdAsync(FriendDto friend)
    {
        return await _getPozdrikId.GetPozdrikIdAsync(friend);
    }
    
    
}