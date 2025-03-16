namespace Domain.Interfaces.ITgSubProxy;

public interface ITgSubProxy
{
    Task SendTgSubVerification(string message);
}