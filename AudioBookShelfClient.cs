using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class AudioBookShelfClient
{
    private readonly string _apiRootUrl;
    private readonly HttpClient _httpClient;
    private string AuthToken = string.Empty;

    public AudioBookShelfClient(string apiRootUrl, string apiToken)
    {
        _apiRootUrl = apiRootUrl;
        _httpClient = new HttpClient();
        AuthToken = apiToken;
    }

    public async Task<List<AbsLibrary>> GetLibrariesAsync()
    {
        var librariesUrl = $"{_apiRootUrl}/libraries";
        var request = new HttpRequestMessage(HttpMethod.Get, librariesUrl);
        request.Headers.Add("Authorization", $"Bearer {AuthToken}");

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getLibrariesResponse = JsonSerializer.Deserialize<AbsGetLibrariesResponse>(responseBody, options);
            return getLibrariesResponse?.Libraries ?? new List<AbsLibrary>();
        }
        else
        {
            throw new HttpRequestException($"Failed to get libraries with status code {response.StatusCode}");
        }
    }

    public async Task<List<AbsSeriesBooks>> GetSeriesAsync(string libraryId)
    {
        var seriesUrl = $"{_apiRootUrl}/Libraries/{libraryId}/series?limit=100";
        var request = new HttpRequestMessage(HttpMethod.Get, seriesUrl);
        request.Headers.Add("Authorization", $"Bearer {AuthToken}");

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getSeriesResponse = JsonSerializer.Deserialize<AbsGetSeriesResponse>(responseBody, options);
            return getSeriesResponse?.Results ?? new List<AbsSeriesBooks>();
        }
        else
        {
            throw new HttpRequestException($"Failed to get series with status code {response.StatusCode}");
        }
    }

    public async Task<bool> UpdateBookSeriesAsync(string bookId, string seriesName, string seriesSequence)
    {
        var displayName = $"{seriesName} #{seriesSequence}";
        var updateBook = new AbsUpdateBookParameter
        {
            Metadata = new AbsUpdateBookMetaData
            {
                Series = new List<AbsUpdateBookSeriesSequence>
                {
                    new AbsUpdateBookSeriesSequence
                    {
                        DisplayName = displayName,
                        Name = seriesName,
                        Sequence = seriesSequence
                    }
                }
            }
        };
        var options = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var updateUrl = $"{_apiRootUrl}/items/{bookId}/media";
        var request = new HttpRequestMessage(HttpMethod.Patch, updateUrl);
        request.Headers.Add("Authorization", $"Bearer {AuthToken}");
        var jsonContent = JsonSerializer.Serialize(updateBook, options);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

}