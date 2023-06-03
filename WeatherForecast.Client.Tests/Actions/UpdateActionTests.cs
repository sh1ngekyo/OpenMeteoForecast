using MediatR;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

using UserRemoteApi.Commands.UpdateUser;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.States.Actions;
using WeatherForecast.Client.Core.Domain.Models;

using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.Actions
{
    public class UpdateActionTests
    {

        [Fact]
        public async Task UpdateActionTests_Success()
        {
            var _userManageService = new Mock<IUserManageService>();
            _userManageService.Setup(
                _ => _.HandleAsync(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new Response<Unit>()
                {
                    Result = new Unit()
                });
            var got = await new UpdateAction(_userManageService.Object).Do(
                new Message()
                {
                    Chat = new Chat() { Id = UserStateContextFactory.UsersID.Values.Max() + 1 },
                    Location = new Location() { Latitude = 11, Longitude = 11 }
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.NotNull(got.Result);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
            Assert.Equal(Core.Domain.Models.Enums.MessageType.UpdateOk, got.Result!.Response);
        }
    }
}
