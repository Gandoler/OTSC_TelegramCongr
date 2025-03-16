using System.Net.Http.Json;
using Domain.Interfaces.IDBProxy;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Data.DBProxy;

public class GetTodayBDayFriends : IGetTodayBDayFriends
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GetTodayBDayFriends(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<FriendDto>> GetTodayBDayFriendsAsync()
    {
        string requestUrl = "api/tgbot/birthdays/today";
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
            _logger.Warning("Request to {Url} failed with status code {StatusCode}: {ReasonPhrase}", 
                requestUrl, (int)response.StatusCode, response.ReasonPhrase);
            throw new HttpRequestException($"Error fetching birthdays: {response.ReasonPhrase}");
        }

        List<FriendDto>? responseBody = await response.Content.ReadFromJsonAsync<List<FriendDto>>();

        if (responseBody == null)
        {
            _logger.Warning("Response from {Url} is null", requestUrl);
            return new List<FriendDto>(); // Возвращаем пустой список, чтобы избежать NullReferenceException
        }

        _logger.Information("Successfully retrieved {Count} friends with birthdays today", responseBody.Count);
        return responseBody;
    }
}