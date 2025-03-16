using Domain.DTO.TG;
using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetTgId
{
    Task<TgIdDto?> GetTgIdAsync(AppIdDto appId);
}