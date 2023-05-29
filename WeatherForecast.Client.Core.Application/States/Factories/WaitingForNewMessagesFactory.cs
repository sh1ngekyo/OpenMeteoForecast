using OpenMeteoRemoteApi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States.Factories
{
    public class WaitingForNewMessagesFactory : IStateFactory
    {
        private readonly IUserManageService _userManageService;
        private readonly IWeatherApiWrapper _weatherApi;

        public WaitingForNewMessagesFactory(IUserManageService userManageService, IWeatherApiWrapper weatherApi)
        {
            _userManageService = userManageService;
            _weatherApi = weatherApi;
        }

        public bool AppliesTo(StateType stateType)
        {
            return Enum.GetName(stateType).Equals(nameof(WaitingForNewMessages));
        }

        public IState CreateState(StateType stateType)
        {
            return new WaitingForNewMessages(_userManageService, _weatherApi);
        }
    }
}
