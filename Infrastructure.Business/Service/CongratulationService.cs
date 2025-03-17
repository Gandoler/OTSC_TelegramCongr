using Domain.DTO.TG;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Entities.Templates;

namespace Interfaces.Business.Service;

public class CongratulationService: ICongratulationService
{
    private readonly ITgBotRepository _tgBotRepository;

    public CongratulationService(ITgBotRepository tgBotRepository)
    {
        _tgBotRepository = tgBotRepository;
    }

    public async Task<IEnumerable<(long? ChatId, string Text)>> GetCongratulationsAsync()
    {
        List<FriendDto> BDayList = await _tgBotRepository.GetTodayBDayFriendsAsync();
        var result = new List<(long?, string)>();
        
        foreach (var friend in BDayList)
        {
            PozdrikIdDto? id = await _tgBotRepository.GetPozdrikIdAsync(friend);
            if (id == null) continue;
            
            string? congratulation = await _tgBotRepository.GetCongratulationAsync(id);
            if (string.IsNullOrEmpty(congratulation)) continue;
            
            TgIdDto? tgid = await _tgBotRepository.GetTgIdAsync(new AppIdDto { AppId = friend.AppId });
            if (tgid?.TgId == null) continue;
            
            result.Add((tgid.TgId, congratulation));
        }
        
        return result;
    }
}