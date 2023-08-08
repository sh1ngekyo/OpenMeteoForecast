using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States.Commands.UpdateUserState
{
    public class UpdateUserStateCommand : IRequest<Unit>
    {
        public long Id { get; set; }
        public StateType StateType { get; set; }
    }
}
