using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Application.States;
using WeatherForecast.Client.Core.Application.States.Factories;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Tests.States.Factories
{
    public class StateStrategyTests
    {
        private IStateStrategy CreateStrategy() => new StateStrategy(new IStateFactory[]
            {
                new WaitingForLocationFactory(null!),
                new WaitingForNewMessagesFactory(null!, null!),
                new WaitingForTimeSpanFactory(null!),
                new WaitingForTimeIntervalFactory(null!),
            });

        [Theory]
        [InlineData(StateType.WaitingForNewMessages)]
        [InlineData(StateType.WaitingForLocation)]
        [InlineData(StateType.WaitingForTimeInterval)]
        [InlineData(StateType.WaitingForTimeSpan)]
        public void StateStrategyTests_Success(StateType stateType)
        {
            var avaliableTypes = new[]
            {
                typeof(WaitingForNewMessages),
                typeof(WaitingForLocation),
                typeof(WaitingForTimeInterval),
                typeof(WaitingForTimeSpan),
            };
            var state = CreateStrategy().CreateState(stateType);
            Assert.IsType(avaliableTypes.First(x => x.Name.Equals(stateType.ToString())), state);
        }
    }
}
