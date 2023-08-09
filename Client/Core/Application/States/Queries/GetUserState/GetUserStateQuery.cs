using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.States.Queries.GetUserState
{
    public class GetUserStateQuery : IRequest<UserState>
    {
        public long Id { get; set; }
    }
}
