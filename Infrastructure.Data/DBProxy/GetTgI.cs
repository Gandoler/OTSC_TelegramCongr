using System.Net.Http.Json;
using Domain.DTO.TG;
using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Data.DBProxy;

public class GetTgI: IGetTgId
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GetTgI(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<TgIdDto?> GetTgId(AppIdDto appId)
    {
        

        _logger.Information($"Starting GetTgId request for AppId: {appId.AppId}");

        try
        {
            var response = await _httpClient.GetAsync($"api/tgbot/tgid/{appId.AppId}");
        
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning($"GetTgId request for AppId {appId.AppId} failed with status {response.StatusCode}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<TgIdDto>();

            if (result is null)
            {
                _logger.Warning($"GetTgId response for AppId {appId.AppId} is null");
            }
            else
            {
                _logger.Information($"GetTgId request for AppId {appId.AppId} succeeded: {result}");
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, $"GetTgId request for AppId {appId.AppId} encountered an HTTP error");
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Unexpected error during GetTgId request for AppId {appId.AppId}");
            return null;
        }
    }

}