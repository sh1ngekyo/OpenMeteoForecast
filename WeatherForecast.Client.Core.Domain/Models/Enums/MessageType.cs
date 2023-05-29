using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Domain.Models.Enums
{
    public enum MessageType
    {
        RegisterOk,
        UpdateOk,
        LocationRequest,
        ForecastNow,
        ForecastToday,
        ForecastNext,
        ForecastWeek,
        TimeSpanRequest,
        TimeSpanRequestOk,
        TimeIntervalRequest,
        AlarmOff,
        WarningsOn,
        WarningsOff,
        WarningMessage
    }
}
