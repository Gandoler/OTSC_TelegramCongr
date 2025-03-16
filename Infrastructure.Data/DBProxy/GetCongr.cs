using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;


namespace Infrastructure.Data.DBProxy;

public class GetCongr: IGetCongr
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GetCongr(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<string?> GetCongratulationAsync(PozdrikIdDto pozdrikId)
    {
        string requestUrl = $"api/tgbot/getCongrByPID/{pozdrikId}";
        _logger.Information("Sending request to {Url}", requestUrl);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(requestUrl);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Exception occurred while making request to {Url}", requestUrl);
            throw;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.Warning("Request to {Url} failed with status code {StatusCode}: {ReasonPhrase}", requestUrl, (int)response.StatusCode, response.ReasonPhrase);
            throw new Exception($"Error fetching Name: {response.ReasonPhrase}");
        }
        
        string responseBody = await response.Content.ReadAsStringAsync();
        _logger.Information("Received response from {Url}: {Response}", requestUrl, responseBody);
        
        return responseBody;
    }
}