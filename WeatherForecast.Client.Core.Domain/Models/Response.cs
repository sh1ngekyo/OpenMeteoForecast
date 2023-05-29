using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Domain.Models
{
    public class Response<T>
    {
        public T? Result { get; set; }
        public bool HasError => Error != ErrorMessageType.None;
        public ErrorMessageType Error { get; set; } = ErrorMessageType.None;
    }
}
