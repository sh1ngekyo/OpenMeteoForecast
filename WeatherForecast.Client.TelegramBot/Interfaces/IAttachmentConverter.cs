using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.TelegramBot.Interfaces
{
    public interface IAttachmentConverter
    {
        public Task<string> Convert(Attachment attachment);
    }
}
