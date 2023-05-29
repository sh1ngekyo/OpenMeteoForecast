using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Notifications;
using WeatherForecast.Client.TelegramBot.Telegram;

namespace WeatherForecast.Client.TelegramBot
{
    public class NotificationBackgroundService : BackgroundService
    {
        public NotificationBackgroundService(TelegramClient client, AlarmNotificationService alarmNotificationService, WarningsNotificationService warningsNotificationService)
        {
            _client = client;
            _alarmNotificationService = alarmNotificationService;
            _warningsNotificationService = warningsNotificationService;
        }

        private readonly TelegramClient _client;
        private readonly AlarmNotificationService _alarmNotificationService;
        private readonly WarningsNotificationService _warningsNotificationService;

        private bool TryInvokeWarnings(int hours) => DateTime.Now.Hour != hours;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var hours = -1;
            while (!stoppingToken.IsCancellationRequested)
            {
                var notifications = await _alarmNotificationService.NotifyAll(stoppingToken);
                await _client.HandleNotifications(notifications, stoppingToken);
                if (TryInvokeWarnings(hours))
                {
                    hours = DateTime.Now.Hour;
                    var warnings = await _warningsNotificationService.NotifyAll(stoppingToken);
                    await _client.HandleNotifications(warnings, stoppingToken);
                    await _client.UpdateConnection();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            await Task.CompletedTask;
        }
    }
}
