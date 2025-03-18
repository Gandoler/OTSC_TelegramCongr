using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.DTO.TG;
using Domain.Interfaces.ITgSubProxy;
using Serilog;

namespace Interfaces.Business.TgSubProxy;

public class TgSubProxy : ITgSubProxy
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public TgSubProxy(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClient = httpClientFactory.CreateClient("TgSubClient");
        _logger = logger;
    }

    public async Task SendTgSubVerification(TgIdAndToken tgIdAndToken)
    {
        _logger.Information("Отправка верификации Telegram ID {TgId}", tgIdAndToken.TgId);

        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/TgSub/subscribeForUserOnBot", tgIdAndToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.Information("Успешно отправлена верификация для Telegram ID {TgId}", tgIdAndToken.TgId);
            }
            else
            {
                _logger.Warning("Ошибка при отправке верификации для Telegram ID {TgId}. Код ответа: {StatusCode}, Причина: {ReasonPhrase}",
                    tgIdAndToken.TgId, response.StatusCode, response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ошибка при отправке верификации для Telegram ID {TgId}", tgIdAndToken.TgId);
            throw;
        }
    }
}