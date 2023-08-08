using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States
{
    public class StateStrategy : IStateStrategy
    {
        private readonly IStateFactory[] _stateFactories;

        public StateStrategy(IStateFactory[] stateFactories)
        {
            _stateFactories = stateFactories;
        }

        public IState CreateState(StateType stateType)
        {
            var stateFactory = _stateFactories.FirstOrDefault(factory => factory.AppliesTo(stateType));
            if (stateFactory == null)
            {
                throw new ArgumentException();
            }
            return stateFactory.CreateState(stateType);
        }
    }
}
