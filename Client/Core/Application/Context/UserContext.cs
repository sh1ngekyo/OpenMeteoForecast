using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.Context
{
    public class UserContext : IUserContext
    {
        private IState _state;
        private readonly IStateStrategy _stateStrategy;

        public UserContext(IStateStrategy stateStrategy, StateType stateType)
        {
            _stateStrategy = stateStrategy;
            _state = _stateStrategy.CreateState(stateType);
        }

        public async Task<Response<OutputMessage>> Update(Message message, CancellationToken cancellationToken = default)
        {
            return await _state.Handle(this, message, cancellationToken);
        }

        public void UpdateState(StateType stateType)
        {
            _state = _stateStrategy.CreateState(stateType);
        }

        public StateType GetStateType()
        {
            return Enum.Parse<StateType>(_state.GetType().Name);
        }
    }
}
