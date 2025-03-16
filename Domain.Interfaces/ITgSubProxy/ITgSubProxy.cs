using Domain.DTO.TG;

namespace Domain.Interfaces.ITgSubProxy;

public interface ITgSubProxy
{
    Task SendTgSubVerification(TgIdAndToken tgIdAndToken);
}