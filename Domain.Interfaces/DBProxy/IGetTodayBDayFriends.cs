using Entities.Templates;

namespace Domain.Interfaces.DBProxy;

public interface IGetTodayBDayFriends
{
    Task<List<PozdrikIdDto>> GetTodayBDayFriendsAsync();
}