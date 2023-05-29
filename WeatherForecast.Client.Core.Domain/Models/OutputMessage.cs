using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Domain.Models
{
    public class OutputMessage
    {
        public MessageType Response { get; set; }
        public string AdditionalInformation { get; set; } = string.Empty;
        public List<Attachment>? Attachments { get; set; }
    }
}
