using MediatR;
using NSubstitute;

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
    public class DisableWarningsActionTests
    {
        private IUserManageService CreateMockService()
        {
            var _userManageService = Substitute.For<IUserManageService>();
            _userManageService.HandleAsync(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>())
                .Returns(
                new Response<Unit>()
                {
                    Result = new Unit()
                });
            return _userManageService;
        }

        [Fact]
        public async Task DisableWarningsActionTests_Success()
        {
            var expected = Core.Domain.Models.Enums.MessageType.WarningsOff;
            var got = await new DisableWarningsAction(CreateMockService()).Do(
                new Message()
                {
                    Chat = new Chat() { Id = UserStateContextFactory.UsersID.Values.Max() + 1 }
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.NotNull(got.Result);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
            Assert.Equal(expected, got.Result.Response);
        }
    }
}
