using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.Interfaces
{
    public interface IStateFactory
    {
        IState CreateState(StateType stateType);

        bool AppliesTo(StateType stateType);
    }
}
