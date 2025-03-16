namespace Domain.Interfaces.IServices;

public interface ITelegramBotService
{
    Task StartAsync(CancellationToken cancellationToken);
}