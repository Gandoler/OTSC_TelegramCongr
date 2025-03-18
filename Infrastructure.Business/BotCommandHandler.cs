using Domain.DTO.TG;
using Domain.Interfaces;
using Domain.Interfaces.ITgSubProxy;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interfaces.Business;

public class BotCommandHandler: IBotCommandHandler
{
    private readonly ITgSubProxy _tgSubProxy;
    private readonly ILogger _logger;

    public BotCommandHandler(ITgSubProxy tgSubProxy, ILogger logger)
    {
        _tgSubProxy = tgSubProxy;
        _logger = logger;
    }
    
    public async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.Information("Получено сообщение от {UserId}: {MessageText}", message.From?.Id, message.Text);

        if (message.Text != null && (message.Text.StartsWith("/start")|| message.Text.StartsWith("/?start")))
        {
            _logger.Information("Обнаружена команда /start от {UserId}", message.From?.Id);
        
            if (message.From != null)
            {
                var token = message.Text.Length > 6 ? message.Text.Substring(8) : string.Empty; // Обрезаем "/start "
                await _tgSubProxy.SendTgSubVerification(new TgIdAndToken
                    { TgId = message.From.Id, Token = token });
                _logger.Information("Запрос на верификацию Telegram отправлен для {UserId}", message.From.Id);
            }
            else
            {
                _logger.Warning("Сообщение с командой /start не содержит информации о пользователе.");
            }
        }
        else
        {
            _logger.Debug("Сообщение не является командой /start.");
        }
    }

}