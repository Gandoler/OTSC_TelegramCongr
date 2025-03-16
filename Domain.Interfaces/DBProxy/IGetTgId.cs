using Entities.Templates;

namespace Domain.Interfaces.DBProxy;

public interface IGetTgId
{
    Task<RegisterTgDto> GetTgId(AppIdDto appId);
}