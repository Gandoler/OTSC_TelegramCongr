using Telegram.Bot;
using Telegram.Bot.Types;

namespace Domain.Interfaces;

public interface IBotCommandHandler
{
    Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}