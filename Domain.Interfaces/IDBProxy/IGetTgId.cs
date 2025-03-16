using Domain.DTO.TG;
using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetTgId
{
    Task<TgIdDto?> GetTgId(AppIdDto appId);
}