using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetPozdrikId
{
    Task<PozdrikIdDto?> GetPozdrikIdAsync(FriendDto friendDto);
}