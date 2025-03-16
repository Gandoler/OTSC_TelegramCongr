using System.Net.Http.Json;
using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Data.DBProxy;

public class GetPozdrikId : IGetPozdrikId
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GetPozdrikId(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<PozdrikIdDto?> GetPozdrikIdAsync(FriendDto friendDto)
    {
        if (friendDto == null)
        {
            _logger.Warning("GetPozdrikIdAsync: Provided friendDto is null");
            return null;
        }

        try
        {
            var url = $"api/tgbot/getPozdrikId/{friendDto}"; 
            _logger.Information("Sending request to {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
        
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning("GetPozdrikIdAsync request for FriendId {FriendId} failed with status {StatusCode}", friendDto.AppId, response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<PozdrikIdDto>();
            
            if (result == null)
            {
                _logger.Warning("GetPozdrikIdAsync response for FriendId {FriendId} is empty", friendDto.AppId);
                return null;
            }

            _logger.Information("GetPozdrikIdAsync request for FriendId {FriendId} succeeded: {Result}", friendDto.AppId, result);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "GetPozdrikIdAsync request for FriendId {FriendId} encountered an HTTP error", friendDto.AppId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error during GetPozdrikIdAsync request for FriendId {FriendId}", friendDto.AppId);
            return null;
        }
    }
}
