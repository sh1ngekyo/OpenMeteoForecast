using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace OpenMeteoRemoteApi.Models
{
    public class HourlyWeather
    {
        [JsonPropertyName("time")]
        public string[]? Time { get; set; }
        [JsonPropertyName("temperature_2m")]
        public double[]? Temperature { get; set; }
        [JsonPropertyName("weathercode")]
        public WeatherCode[]? WeatherCode { get; set; }
        [JsonPropertyName("rain")]
        public double[]? Rain { get; set; }
        [JsonPropertyName("showers")]
        public double[]? Showers { get; set; }
        [JsonPropertyName("snowfall")]
        public double[]? Snowfall { get; set; }
    }
}
