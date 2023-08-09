using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace OpenMeteoRemoteApi.Models
{
    public class CurrentWeather
    {
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }
        [JsonPropertyName("time")]
        public string? Time { get; set; }
        [JsonPropertyName("weathercode")]
        public WeatherCode WeatherCode { get; set; }
        [JsonPropertyName("windspeed")]
        public double Windspeed { get; set; }
        [JsonPropertyName("winddirection")]
        public double WindDirection { get; set; }
    }
}
