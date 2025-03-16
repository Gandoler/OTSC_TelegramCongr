using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interfaces.Business.Service;

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient _botClient;
    private readonly IBotCommandHandler _commandHandler;
    private readonly ILogger _logger;

    public TelegramBotService(IBotCommandHandler botCommandHandler, TelegramBotClient botClient, ILogger logger)
    {
        _botClient = botClient;
        _commandHandler = botCommandHandler;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Бот запущен...");

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            cancellationToken: cancellationToken
        );

        await Task.Delay(-1, cancellationToken);
    }
    
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || string.IsNullOrEmpty(message.Text))
            return;

        _logger.Information("Получено сообщение от {UserId}: {MessageText}", message.From?.Id, message.Text);
        return await _commandHandler.HandleMessageAsync(botClient, message, cancellationToken);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error(exception, "Ошибка бота");
        return Task.CompletedTask;
    }
}