using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Application.States.Commands.DeleteUserState
{
    public class DeleteUserStateCommand : IRequest<Unit>
    {
        public long Id { get; set; }
    }
}
