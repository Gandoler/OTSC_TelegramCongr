using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetTgId
{
    Task<RegisterTgDto> GetTgId(AppIdDto appId);
}