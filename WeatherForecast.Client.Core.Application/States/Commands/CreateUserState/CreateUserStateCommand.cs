﻿using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Application.States.Commands.CreateUserState
{
    public class CreateUserStateCommand : IRequest<Domain.Models.UserState>
    {
        public long Id { get; set; }
    }
}
