using System.Net.Http.Json;

namespace Sparkasse.Client.Common.Services;

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecastDto>> GetWeatherForecastsAsync();
}

/// <summary>
/// Weather service that retrieves weather forecasts from the WebApi.
/// Uses the "WebApi" HttpClient via IHttpClientFactory.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<WeatherForecastDto>> GetWeatherForecastsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("WebApi");
            var forecasts = await client.GetFromJsonAsync<IEnumerable<WeatherForecastDto>>("/api/WeatherForecasts");
            return forecasts ?? Enumerable.Empty<WeatherForecastDto>();
        }
        catch (Exception)
        {
            // Log error or handle appropriately
            return Enumerable.Empty<WeatherForecastDto>();
        }
    }
}

/// <summary>
/// DTO compatible with WeatherForecast from Application project.
/// </summary>
public class WeatherForecastDto
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string Summary { get; set; } = string.Empty;
}
