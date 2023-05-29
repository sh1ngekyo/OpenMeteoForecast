﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States.Factories
{
    public class WaitingForTimeIntervalFactory : IStateFactory
    {
        private readonly IUserManageService _userManageService;

        public WaitingForTimeIntervalFactory(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        public bool AppliesTo(StateType stateType)
        {
            return Enum.GetName(stateType).Equals(nameof(WaitingForTimeInterval));
        }

        public IState CreateState(StateType stateType)
        {
            return new WaitingForTimeInterval(_userManageService);
        }
    }
}
