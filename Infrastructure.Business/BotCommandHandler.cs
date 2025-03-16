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
        if (message.Text != null && message.Text.StartsWith("/start"))
        {
            
            await _tgSubProxy.SendTgSubVerification(new TgIdAndToken{Id=message.From.Id,Token=message.Text});
            _logger.Information("Запрос на верификацию Telegram отправлен для {UserId}", message.From.Id);

        }
    }
}