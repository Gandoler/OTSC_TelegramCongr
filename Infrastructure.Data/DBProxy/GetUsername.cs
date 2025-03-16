using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Data.DBProxy;

public class GetUsername : IGetUsername
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GetUsername(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<string?> GetUsernameAsync(PozdrikIdDto pozdrikId)
    {
        try
        {
            var url = $"api/tgbot/getUsernameByPId/{pozdrikId._pozdrikId}";
            _logger.Information("Sending request to {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
        
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning("GetUsernameAsync request for PozdrikId {PozdrikId} failed with status {StatusCode}", pozdrikId._pozdrikId, response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(result))
            {
                _logger.Warning("GetUsernameAsync response for PozdrikId {PozdrikId} is empty", pozdrikId._pozdrikId);
                return null;
            }

            _logger.Information("GetUsernameAsync request for PozdrikId {PozdrikId} succeeded: {Result}", pozdrikId._pozdrikId, result);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "GetUsernameAsync request for PozdrikId {PozdrikId} encountered an HTTP error", pozdrikId._pozdrikId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error during GetUsernameAsync request for PozdrikId {PozdrikId}", pozdrikId._pozdrikId);
            return null;
        }
    }
}