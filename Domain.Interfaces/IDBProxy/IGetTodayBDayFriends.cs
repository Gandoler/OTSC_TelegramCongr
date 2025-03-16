using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetTodayBDayFriends
{
    Task<List<FriendDto>> GetTodayBDayFriendsAsync();
}