using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.States.Actions
{
    public class EnableWarningsAction
    {
        private class TimeInterval
        {
            public TimeSpan Start { get; set; }
            public TimeSpan End { get; set; }
        }

        private readonly IUserManageService _userManageService;
        private readonly string _timeFormat = "HH:mm";

        public EnableWarningsAction(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        private void PadTime(ref string time)
        {
            while (time.Length < _timeFormat.Length)
            {
                time = time.PadLeft(_timeFormat.Length, '0');
            }
        }

        private bool TryParseInterval(string time, out TimeInterval TimeInterval)
        {
            TimeInterval = null;
            (DateTime Start, DateTime End) DateInterval;
            var splittedRawTime = time.Split('-');
            if (splittedRawTime.Length != 2)
            {
                return false;
            }
            PadTime(ref splittedRawTime[0]);
            PadTime(ref splittedRawTime[1]);
            if (!DateTime.TryParseExact(splittedRawTime[0], _timeFormat, CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None, out DateInterval.Start))
            {
                return false;
            }
            if (!DateTime.TryParseExact(splittedRawTime[1], _timeFormat, CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None, out DateInterval.End))
            {
                return false;
            }
            TimeInterval = new TimeInterval()
            {
                Start = DateInterval.Start.TimeOfDay,
                End = DateInterval.End.TimeOfDay
            };
            return true;
        }

        public async Task<Response<OutputMessage>> Do(Message message, CancellationToken cancellationToken = default)
        {
            message.Text = message!.Text!.ToLowerInvariant();
            if (!TryParseInterval(message.Text, out var interval))
            {
                return new Response<OutputMessage>()
                {
                    Error = ErrorMessageType.WrongTimeFormat
                };
            }
            var response = await _userManageService.HandleAsync(
                new UpdateUserCommand
                {
                    Id = message.Chat.Id,
                    WarningsStartTime = new UserRemoteApi.Models.Time()
                    {
                        Value = interval.Start
                    },
                    WarningsEndTime = new UserRemoteApi.Models.Time()
                    {
                        Value = interval.End
                    }
                }, cancellationToken);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = MessageType.WarningsOn
                },
                Error = response.Error
            };
        }
    }
}
