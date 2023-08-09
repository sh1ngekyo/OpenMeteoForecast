using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.Interfaces
{
    public interface IState
    {
        Task<Response<OutputMessage>> Handle(IUserContext context, Message message, CancellationToken cancellationToken = default);
    }
}
