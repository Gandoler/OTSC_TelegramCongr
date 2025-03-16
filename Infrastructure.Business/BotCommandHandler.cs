using Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interfaces.Business;

public class BotCommandHandler: IBotCommandHandler
{
    public Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}