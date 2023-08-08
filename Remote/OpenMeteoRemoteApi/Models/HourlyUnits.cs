using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenMeteoRemoteApi.Models
{
    public class HourlyUnits
    {
        [JsonPropertyName("time")]
        public string? TimeStandart { get; set; }
        [JsonPropertyName("temperature_2m")]
        public string? Temperature_2m { get; set; }
        [JsonPropertyName("weathercode")]
        public string? WeatherCode { get; set; }
        [JsonPropertyName("windspeed")]
        public string? WindSpeed { get; set; }
        [JsonPropertyName("snowfall")]
        public string? Snowfall { get; set; }
        [JsonPropertyName("rain")]
        public string? Rain { get; set; }
        [JsonPropertyName("showers")]
        public string? Showers { get; set; }
    }
}
