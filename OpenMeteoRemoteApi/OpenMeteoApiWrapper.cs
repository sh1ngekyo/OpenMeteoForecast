using HttpService;

using Microsoft.Extensions.Configuration;

using OpenMeteoRemoteApi.Interfaces;
using OpenMeteoRemoteApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace OpenMeteoRemoteApi
{
    public class OpenMeteoApiWrapper : IWeatherApiWrapper
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public OpenMeteoApiWrapper(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<Response<WeatherResponse?>> GetWeatherAsync(WeatherRequest request, CancellationToken cancellationToken = default)
        {
            var queryString = $@"{_configuration.GetRequiredSection("ApiPaths")["OpenMeteo"]}latitude={request.Location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&longitude={request.Location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&current_weather=true&timezone=auto&forecast_days={request.DaysInterval}&hourly=temperature_2m";
            if (request.Details)
                queryString += ",rain,showers,snowfall,weathercode";
            var response = await _httpService.GetAsync(queryString, cancellationToken);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                return new Response<WeatherResponse?>()
                {
                    Error = ErrorMessageType.WeatherServiceUnavaliable
                };
            }
            return new Response<WeatherResponse?>()
            {
                Result = await response.Content.ReadFromJsonAsync<WeatherResponse?>()
            };
        }
    }
}
