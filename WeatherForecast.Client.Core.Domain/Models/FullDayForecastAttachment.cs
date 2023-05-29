using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Domain.Models
{
    public class FullDayForecastAttachment : Attachment
    {
        public (DateTime Time, double Temperature, WeatherCode WeatherCode)[] DailyForecast { get; set; }
    }
}
