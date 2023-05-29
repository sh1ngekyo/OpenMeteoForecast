using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenMeteoRemoteApi.Models
{
    public class WeatherResponse
    {
        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }
        [JsonPropertyName("generationtime_ms")]
        public double? GenerationTime { get; set; }
        [JsonPropertyName("utc_offset_seconds")]
        public double? UtcOffsetSeconds { get; set; }
        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
        [JsonPropertyName("timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }
        [JsonPropertyName("hourly_units")]
        public HourlyUnits? HourlyUnits { get; set; }
        [JsonPropertyName("hourly")]
        public HourlyWeather? HourlyWeather { get; set; }
        [JsonPropertyName("current_weather")]
        public CurrentWeather? CurrentWeather { get; set; }
    }
}
