using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Domain.Models
{
    public class WeekForecastAttachment : Attachment
    {
        public (DateTime Day, double AvgTemperature, WeatherCode WeatherCode)[] WeeklyForecast { get; set; }
    }
}
