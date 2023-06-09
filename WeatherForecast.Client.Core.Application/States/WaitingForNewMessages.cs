﻿using OpenMeteoRemoteApi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Application.States.Actions;

namespace WeatherForecast.Client.Core.Application.States
{
    public class WaitingForNewMessages : IState
    {
        private readonly IUserManageService _userManageService;
        private readonly IWeatherApiWrapper _weatherApi;

        public WaitingForNewMessages(IUserManageService userManageService, IWeatherApiWrapper weatherApi)
        {
            _userManageService = userManageService;
            _weatherApi = weatherApi;
        }

        private Response<OutputMessage> RequestLocation(IUserContext context)
        {
            context.UpdateState(StateType.WaitingForLocation);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = Domain.Models.Enums.MessageType.LocationRequest
                }
            };
        }

        private Response<OutputMessage> RequestTime(IUserContext context)
        {
            context.UpdateState(StateType.WaitingForTimeSpan);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = Domain.Models.Enums.MessageType.TimeSpanRequest
                }
            };
        }

        private Response<OutputMessage> RequestTimeInterval(IUserContext context)
        {
            context.UpdateState(StateType.WaitingForTimeInterval);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = Domain.Models.Enums.MessageType.TimeIntervalRequest
                }
            };
        }

        private async Task<Response<OutputMessage>> HandleText(IUserContext context, Message message, CancellationToken cancellationToken = default)
        {
            switch (message.Text!.ToLowerInvariant())
            {
                case "/register":
                case "/update":
                    return RequestLocation(context);
                case "/forecast":
                    return await new GetForecastAction(
                        ForecastRequest.Now,
                        _userManageService,
                        _weatherApi).Do(message, cancellationToken);
                case "/forecast_day":
                    return await new GetForecastAction(
                        ForecastRequest.Day,
                        _userManageService,
                        _weatherApi).Do(message, cancellationToken);
                case "/forecast_next":
                    return await new GetForecastAction(
                        ForecastRequest.Next,
                        _userManageService,
                        _weatherApi).Do(message, cancellationToken);
                case "/forecast_week":
                    return await new GetForecastAction(
                        ForecastRequest.Week,
                        _userManageService,
                        _weatherApi).Do(message, cancellationToken);
                case "/alarm_on":
                    return RequestTime(context);
                case "/alarm_off":
                    return await new RemoveAlarmAction(
                        _userManageService).Do(message, cancellationToken);
                case "/warnings_on":
                    return RequestTimeInterval(context);
                case "/warnings_off":
                    return await new DisableWarningsAction(
                        _userManageService).Do(message, cancellationToken);
                default:
                    return new Response<OutputMessage>()
                    {
                        Error = ErrorMessageType.UnknownCommandType
                    };
            }
        }

        public async Task<Response<OutputMessage>> Handle(IUserContext context, Message message, CancellationToken cancellationToken = default) => message.Type switch
        {
            Telegram.Bot.Types.Enums.MessageType.Text => await HandleText(context, message, cancellationToken),
            _ => new Response<OutputMessage>()
            {
                Error = Domain.Models.Enums.ErrorMessageType.UnknownMessageType
            }
        };
    }
}
