using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetUsername
{
    Task<string?> GetUsernameAsync(PozdrikIdDto pozdrikId);
}