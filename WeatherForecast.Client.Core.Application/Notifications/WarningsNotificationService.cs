using OpenMeteoRemoteApi.Interfaces;
using OpenMeteoRemoteApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRemoteApi.Interfaces;
using UserRemoteApi.Queries.GetUsers;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models.Notifications;

using WeatherForecast.Client.Core.Domain.Models;
using UserRemoteApi.Models;
using WeatherForecast.Client.Core.Application.Extensions;

namespace WeatherForecast.Client.Core.Application.Notifications
{
    public class WarningsNotificationService : INotificationService
    {
        private readonly IUserManageService _userManageService;
        private readonly IWeatherApiWrapper _weatherApi;
        private const int _warningsHoursDelay = 3;
        private int _warningsCurrentHour;

        private readonly WeatherCode[] _exceptWeatherCodes = new WeatherCode[]
        {
            WeatherCode.ClearSky,
            WeatherCode.MainlyClearSky,
            WeatherCode.PartlyCloudySky,
            WeatherCode.OvercastSky,
            WeatherCode.Fog,
            WeatherCode.DepositingRimeFog,
        };

        public WarningsNotificationService(IUserManageService userManageService, IWeatherApiWrapper weatherApi)
        {
            _userManageService = userManageService;
            _weatherApi = weatherApi;
            _warningsCurrentHour = DateTime.Now.Hour - _warningsHoursDelay;
        }

        private async Task<Response<OutputMessage>> GetWeather(UserViewModel user, CancellationToken cancellationToken)
        {
            var weather = await _weatherApi.GetWeatherAsync(new WeatherRequest()
            {
                DaysInterval = 1,
                Details = true,
                Location = new()
                {
                    Latitude = user!.Latitude,
                    Longitude = user!.Longitude,
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
                Result = weather.Result.Parse(ForecastRequest.Day)
            };
        }

        private bool IsForecastContainsBadWather(Response<OutputMessage> weather, out string additionalInfo)
        {
            additionalInfo = string.Empty;
            if (weather.HasError)
                return false;
            var forecast = (weather!.Result.Attachments.FirstOrDefault() as FullDayForecastAttachment).DailyForecast;
            for (int i = DateTime.Now.Hour; i < DateTime.Now.Hour + _warningsHoursDelay; i++)
            {
                if (!_exceptWeatherCodes.Contains(forecast[i].WeatherCode))
                {
                    additionalInfo += $"{forecast[i].Time.ToShortTimeString()} {forecast[i].WeatherCode.FromCodeToString()}\n";
                }
            }
            return !string.IsNullOrEmpty(additionalInfo);
        }

        public async Task<List<Notification>> NotifyAll(CancellationToken cancellationToken = default)
        {
            var notifications = new List<Notification>();
            var usersList = await _userManageService.HandleAsync(new GetUsersQuery(), cancellationToken);
            if (!usersList.HasError)
            {
                foreach (var user in usersList?.Result?.Users!)
                {
                    if (IsUserShouldBeNotified(user))
                    {
                        var response = await GetWeather(user, cancellationToken);
                        if (IsForecastContainsBadWather(response, out var info))
                        {
                            notifications.Add(new Notification()
                            {
                                UserId = user.Id,
                                Response = new Response<OutputMessage>()
                                {
                                    Result = new OutputMessage()
                                    {
                                        Response = MessageType.WarningMessage,
                                        AdditionalInformation = info
                                    }
                                }
                            });
                        }
                    }
                }
            }
            if (DateTime.Now.Hour == _warningsCurrentHour + _warningsHoursDelay)
            {
                _warningsCurrentHour = DateTime.Now.Hour;
            }
            return notifications;
        }

        private bool IsUserShouldBeNotified(UserViewModel user)
        {
            if (!user.WarningsStartTime.HasValue
                || !user.WarningsEndTime.HasValue)
            {
                return false;
            }
            if (DateTime.Now.Hour < user.WarningsStartTime.Value.Hours
                || DateTime.Now.Hour > user.WarningsEndTime.Value.Hours)
            {
                return false;
            }
            return DateTime.Now.Hour == _warningsCurrentHour + _warningsHoursDelay;
        }
    }
}
