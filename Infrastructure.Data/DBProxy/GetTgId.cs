using System.Net.Http.Json;
using Domain.DTO.TG;
using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Data.DBProxy;

public class GetTgId: IGetTgId
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GetTgId(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<TgIdDto?> GetTgIdAsync(AppIdDto appId)
    {
        

        _logger.Information($"Starting GetTgIdAsync request for AppId: {appId.AppId}");

        try
        {
            var response = await _httpClient.GetAsync($"api/tgbot/tgid/{appId.AppId}");
        
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning($"GetTgIdAsync request for AppId {appId.AppId} failed with status {response.StatusCode}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<TgIdDto>();

            if (result is null)
            {
                _logger.Warning($"GetTgIdAsync response for AppId {appId.AppId} is null");
            }
            else
            {
                _logger.Information($"GetTgIdAsync request for AppId {appId.AppId} succeeded: {result}");
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, $"GetTgIdAsync request for AppId {appId.AppId} encountered an HTTP error");
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Unexpected error during GetTgIdAsync request for AppId {appId.AppId}");
            return null;
        }
    }

}