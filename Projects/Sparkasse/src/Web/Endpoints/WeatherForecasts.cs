using Microsoft.AspNetCore.Http.HttpResults;
using Sparkasse.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace Sparkasse.Web.Endpoints;

public class WeatherForecasts : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization("CookieOrBearer");

        groupBuilder.MapGet(GetWeatherForecasts);
    }

    [EndpointSummary("Get Weather Forecasts")]
    [EndpointDescription("Retrieves a list of weather forecasts for the next few days.")]
    public static async Task<Ok<IEnumerable<WeatherForecast>>> GetWeatherForecasts(ISender sender)
    {
        var forecasts = await sender.Send(new GetWeatherForecastsQuery());

        return TypedResults.Ok(forecasts);
    }
}
