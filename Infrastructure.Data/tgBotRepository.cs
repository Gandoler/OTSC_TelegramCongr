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
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public TgBotRepository(IGetCongr getCongr, IGetTgId getTgId, IGetTodayBDayFriends getTodayBDayFriends,
        HttpClient httpClient, ILogger logger)
    {
        _getCongr = getCongr;
        _getTgId = getTgId;
        _getTodayBDayFriends = getTodayBDayFriends;
        _httpClient = httpClient;
        _logger = logger;
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