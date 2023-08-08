using OpenMeteoRemoteApi.Interfaces;
using OpenMeteoRemoteApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using UserRemoteApi.Interfaces;

using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models;
using UserRemoteApi.Queries.GetUser;
using WeatherForecast.Client.Core.Application.Extensions;

namespace WeatherForecast.Client.Core.Application.States.Actions
{
    public class GetForecastAction
    {
        private readonly ForecastRequest _forecastRequest;
        private readonly IUserManageService _userManageService;
        private readonly IWeatherApiWrapper _weatherApi;

        public GetForecastAction(ForecastRequest forecastRequest, IUserManageService userManageService, IWeatherApiWrapper weatherApi)
        {
            _forecastRequest = forecastRequest;
            _userManageService = userManageService;
            _weatherApi = weatherApi;
        }

        private int GetRequestedInterval() =>
            _forecastRequest switch
            {
                ForecastRequest.Day => 1,
                ForecastRequest.Next => 2,
                ForecastRequest.Week => 7,
                _ => 1
            };

        public async Task<Response<OutputMessage>> Do(Message message, CancellationToken cancellationToken = default)
        {
            var user = await _userManageService.HandleAsync(
                new GetUserQuery
                {
                    Id = message.Chat.Id
                },
                cancellationToken);
            if (user.HasError)
            {
                return new Response<OutputMessage>()
                {
                    Error = user.Error
                };
            }
            var weather = await _weatherApi.GetWeatherAsync(new WeatherRequest()
            {
                DaysInterval = GetRequestedInterval(),
                Details = true,
                Location = new()
                {
                    Latitude = user.Result!.Latitude,
                    Longitude = user.Result!.Longitude,
                }
            });
            if (weather.HasError)
            {
                return new Response<OutputMessage>()
                {
                    Error = weather.Error
                };
            }
            return new Response<OutputMessage>()
            {
                Result = weather.Result.Parse(_forecastRequest)
            };
        }
    }
}
