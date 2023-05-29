﻿using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Backend.Core.Application.Users.Queries.GetUsers
{
    public class GetUserListQuery : IRequest<UserListViewModel>
    {
    }
}
