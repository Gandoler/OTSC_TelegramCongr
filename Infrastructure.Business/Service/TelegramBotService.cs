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
    private readonly ITgBotRepository _tgBotRepository;

    public TelegramBotService(IBotCommandHandler botCommandHandler, TelegramBotClient botClient, 
        ILogger logger, ITgBotRepository tgBotRepository)
    {
        _botClient = botClient;
        _commandHandler = botCommandHandler;
        _logger = logger;
        _tgBotRepository = tgBotRepository;
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
        List<FriendDto> BDayList = await _tgBotRepository.GetTodayBDayFriendsAsync();
    
        foreach (var friend in BDayList)
        {
            string username = friend.FriendUsername;
            PozdrikIdDto? id = await _tgBotRepository.GetPozdrikIdAsync(friend);
        
            if (id != null)
            {
                string? congratulation = await _tgBotRepository.GetCongratulationAsync(id);
                if (!string.IsNullOrEmpty(congratulation))
                {
                    TgIdDto? tgid = await _tgBotRepository.GetTgId(new AppIdDto { AppId = friend.AppId });

                    if (tgid?.TgId != null)
                    {
                        await _botClient.SendTextMessageAsync(
                            chatId: tgid.TgId,
                            text: congratulation,
                            cancellationToken: cancellationToken
                        );
                    }
                }
            }
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