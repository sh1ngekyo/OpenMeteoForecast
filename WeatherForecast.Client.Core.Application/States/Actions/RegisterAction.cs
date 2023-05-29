using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using UserRemoteApi.Commands.RegisterUser;
using UserRemoteApi.Interfaces;

using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States.Actions
{
    public class RegisterAction
    {
        private readonly IUserManageService _userManageService;

        public RegisterAction(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        public async Task<Response<OutputMessage>> Do(Message message, CancellationToken cancellationToken = default)
        {
            var response = await _userManageService.HandleAsync(
                new RegisterUserCommand
                {
                    Id = message.Chat.Id,
                    Longitude = message.Location.Longitude,
                    Latitude = message.Location.Latitude,
                }, cancellationToken);
            return new Response<OutputMessage>()
            {
                Result = new OutputMessage()
                {
                    Response = MessageType.RegisterOk
                },
                Error = response.Error
            };
        }
    }
}
