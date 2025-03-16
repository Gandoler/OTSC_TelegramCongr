using Domain.DTO.TG;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Entities.Templates;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interfaces.Business.Service;

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient _botClient;
    private readonly IBotCommandHandler _commandHandler;
    private readonly ILogger _logger;
    private readonly ICongratulationService _congratulationService;

    public TelegramBotService(IBotCommandHandler botCommandHandler, TelegramBotClient botClient, 
        ILogger logger, ICongratulationService congratulationService)
    {
        _botClient = botClient;
        _commandHandler = botCommandHandler;
        _logger = logger;
        _congratulationService = congratulationService;
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

    public async Task SendCongratulate(Message message, long tgId, CancellationToken cancellationToken)
    {
        var congratulations = await _congratulationService.GetCongratulationsAsync();
        
        foreach (var (chatId, text) in congratulations)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                cancellationToken: cancellationToken
            );
        }
    }
    
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || string.IsNullOrEmpty(message.Text))
            return;

        _logger.Information("Получено сообщение от {UserId}: {MessageText}", message.From?.Id, message.Text);
        await _commandHandler.HandleMessageAsync(botClient, message, cancellationToken);
        
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error(exception, "Ошибка бота");
        return Task.CompletedTask;
    }
}