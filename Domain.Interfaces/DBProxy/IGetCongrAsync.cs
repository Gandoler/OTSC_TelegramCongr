using Entities.Templates;

namespace Domain.Interfaces.DBProxy;

public interface IGetCongrAsync
{
    Task<string?> GetCongrAsync(PozdrikIdDto pozdrikId);
}