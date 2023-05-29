﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Application.States.Actions;
using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.Core.Domain.Models;
using UserRemoteApi.Queries.GetUser;

namespace WeatherForecast.Client.Core.Application.States
{
    public class WaitingForLocation : IState
    {
        private readonly IUserManageService _userManageService;

        public WaitingForLocation(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        public async Task<Response<OutputMessage>> Handle(IUserContext context, Message message, CancellationToken cancellationToken = default)
        {
            context.UpdateState(StateType.WaitingForNewMessages);
            if (message.Location is null)
            {
                return await context.Update(message, cancellationToken);
            }
            var user = await _userManageService.HandleAsync(
               new GetUserQuery
               {
                   Id = message.Chat.Id
               },
               cancellationToken);
            if (user.HasError)
            {
                if (user.Error == ErrorMessageType.NotFound)
                {
                    return await new RegisterAction(_userManageService).Do(message, cancellationToken);
                }
                return new Response<OutputMessage>()
                {
                    Error = user.Error,
                };
            }
            return await new UpdateAction(_userManageService).Do(message, cancellationToken);
        }
    }
}
