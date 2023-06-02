using MediatR;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using UserRemoteApi;
using UserRemoteApi.Commands.RegisterUser;
using UserRemoteApi.Interfaces;

using WeatherForecast.Client.Core.Application.States.Actions;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.Actions
{
    public class RegisterActionTests
    {

        [Fact]
        public async Task RegisterActionTests_Success()
        {
            var command = new RegisterUserCommand()
            {
                Id = UserStateContextFactory.UsersID.Values.Max() + 1,
                Latitude = 12,
                Longitude = 21,
            };
            var _userManageService = new Mock<IUserManageService>();
            _userManageService.Setup(
                _ => _.HandleAsync(It.IsAny<RegisterUserCommand>(), CancellationToken.None))
                .ReturnsAsync(
                new Response<Unit>() 
                {
                    Result = new Unit() 
                });
            _userManageService.Verify();
            var got = await new RegisterAction(_userManageService.Object).Do(
                new Telegram.Bot.Types.Message()
                {
                    Chat = new Chat() { Id = command.Id },
                    Location = new Location() { Latitude = command.Latitude, Longitude = command.Longitude }
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
        }
    }
}
