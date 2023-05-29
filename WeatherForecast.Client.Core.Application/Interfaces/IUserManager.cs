using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.Interfaces
{
    public interface IUserManager
    {
        Task Add(long userId, CancellationToken cancellationToken = default);
        Task Remove(long userId, CancellationToken cancellationToken = default);
        Task<Response<OutputMessage>> HandleMessage(long userId, Message message, CancellationToken cancellationToken = default);
    }
}
