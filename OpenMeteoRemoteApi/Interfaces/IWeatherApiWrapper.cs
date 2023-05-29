using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;

namespace OpenMeteoRemoteApi.Interfaces
{
    public interface IWeatherApiWrapper
    {
        Task<Response<WeatherResponse?>> GetWeatherAsync(WeatherRequest request, CancellationToken cancellationToken = default);
    }
}
