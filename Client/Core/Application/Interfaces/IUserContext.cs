using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.Core.Domain.Models;
using Telegram.Bot.Types;

namespace WeatherForecast.Client.Core.Application.Interfaces
{
    public interface IUserContext
    {
        StateType GetStateType();
        void UpdateState(StateType stateType);
        Task<Response<OutputMessage>> Update(Message message, CancellationToken cancellationToken = default);
    }
}
