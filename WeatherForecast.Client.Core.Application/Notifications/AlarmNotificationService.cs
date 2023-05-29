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

namespace WeatherForecast.Client.Core.Application.Notifications
{
    public class AlarmNotificationService : INotificationService
    {
        private readonly IUserManageService _userManageService;
        private readonly IWeatherApiWrapper _weatherApi;

        public AlarmNotificationService(IUserManageService userManageService, IWeatherApiWrapper weatherApi)
        {
            _userManageService = userManageService;
            _weatherApi = weatherApi;
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

        public async Task<List<Notification>> NotifyAll(CancellationToken cancellationToken = default)
        {
            var notifications = new List<Notification>();
            var usersList = await _userManageService.HandleAsync(new GetUsersQuery(), cancellationToken);
            if (!usersList.HasError)
            {
                foreach (var user in usersList.Result.Users)
                {
                    if (IsUserShouldBeNotified(user))
                    {
                        var response = await GetWeather(user, cancellationToken);
                        notifications.Add(new Notification()
                        {
                            UserId = user.Id,
                            Response = response
                        });
                    }
                }
            }
            return notifications;
        }

        private bool IsUserShouldBeNotified(UserViewModel user)
        {
            if (!user.Alarm.HasValue)
                return false;
            return DateTime.Now.TimeOfDay.Hours == user.Alarm.Value.Hours
                && DateTime.Now.TimeOfDay.Minutes == user.Alarm.Value.Minutes;
        }

    }
}
