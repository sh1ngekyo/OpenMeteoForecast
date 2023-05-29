using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using UserRemoteApi.Commands.UpdateUser;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States.Actions
{
    public class RemoveAlarmAction
    {
        private readonly IUserManageService _userManageService;

        public RemoveAlarmAction(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        public async Task<Response<OutputMessage>> Do(Message message, CancellationToken cancellationToken = default)
        {
            var response = await _userManageService.HandleAsync(
                 new UpdateUserCommand
                 {
                     Id = message.Chat.Id,
                     Alarm = new UserRemoteApi.Models.Time()
                     {
                         Value = null
                     }
                 }, cancellationToken);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = MessageType.AlarmOff
                },
                Error = response.Error
            };
        }
    }
}
