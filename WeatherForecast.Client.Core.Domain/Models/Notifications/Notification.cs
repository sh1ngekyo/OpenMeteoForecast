using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Domain.Models.Notifications
{
    public class Notification
    {
        public long UserId { get; set; }
        public Response<OutputMessage> Response { get; set; }
    }
}
