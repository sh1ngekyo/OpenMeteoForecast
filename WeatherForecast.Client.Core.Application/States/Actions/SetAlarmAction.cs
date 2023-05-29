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
using UserRemoteApi.Commands.UpdateUser;

namespace WeatherForecast.Client.Core.Application.States.Actions
{
    public class SetAlarmAction
    {
        private readonly IUserManageService _userManageService;
        private readonly string _timeFormat = "HH:mm";

        public SetAlarmAction(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        public async Task<Response<OutputMessage>> Do(Message message, CancellationToken cancellationToken = default)
        {
            DateTime dt;
            message.Text = message!.Text!.ToLowerInvariant();
            while (message.Text.Length < _timeFormat.Length)
            {
                message.Text = message.Text.PadLeft(_timeFormat.Length, '0');
            }
            if (!DateTime.TryParseExact(message.Text, _timeFormat, CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None, out dt))
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
                    Alarm = new UserRemoteApi.Models.Time()
                    {
                        Value = dt.TimeOfDay
                    }
                }, cancellationToken);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = MessageType.TimeSpanRequestOk
                },
                Error = response.Error
            };
        }
    }
}
