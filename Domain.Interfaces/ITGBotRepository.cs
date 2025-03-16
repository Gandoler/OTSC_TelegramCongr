
using Domain.DTO.TG;
using Entities.Templates;

namespace Domain.Interfaces;

public interface ITgBotRepository
{
    Task<string?> GetCongratulationAsync(PozdrikIdDto pozdrikId);
    Task<TgIdDto?> GetTgId(AppIdDto appId);
    Task<List<FriendDto>> GetTodayBDayFriendsAsync();



}   