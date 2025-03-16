using Entities.Templates;

namespace Domain.Interfaces.IDBProxy;

public interface IGetCongr
{
    Task<string?> GetCongratulationAsync(PozdrikIdDto pozdrikId);
}