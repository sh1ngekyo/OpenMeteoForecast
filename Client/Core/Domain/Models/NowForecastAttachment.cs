using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Domain.Models
{
    public class NowForecastAttachment : Attachment
    {
        public double Temperature { get; set; }
        public DateTime Time { get; set; }
        public WeatherCode WeatherCode { get; set; }
    }
}
