using Domain.Interfaces.IServices;

namespace Interfaces.Business.Service;

public class TelegramBotService : ITelegramBotService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}