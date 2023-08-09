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
    public class SetAlarmActionTests
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

        [Theory]
        [InlineData("12:35")]
        [InlineData("07:00")]
        [InlineData("23:23")]
        public async Task SetAlarmActionTests_Success(string time)
        {
            var expected = Core.Domain.Models.Enums.MessageType.TimeSpanRequestOk;
            var got = await new SetAlarmAction(CreateMockService()).Do(
                new Message()
                {
                    Chat = new Chat() { Id = UserStateContextFactory.UsersID.Values.Max() + 1 },
                    Text = time,
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.NotNull(got.Result);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
            Assert.Equal(expected, got.Result.Response);
        }

        [Theory]
        [InlineData("0:01")]
        [InlineData("9:44")]
        [InlineData("5:55")]
        public async Task SetAlarmActionTests_SuccessWithPadLeft(string time)
        {
            var expected = Core.Domain.Models.Enums.MessageType.TimeSpanRequestOk;
            var got = await new SetAlarmAction(CreateMockService()).Do(
                new Message()
                {
                    Chat = new Chat() { Id = UserStateContextFactory.UsersID.Values.Max() + 1 },
                    Text = time,
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.NotNull(got.Result);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
            Assert.Equal(expected, got.Result.Response);
        }

        [Theory]
        [InlineData("24:00")]
        [InlineData("9:00:AM")]
        [InlineData("some text")]
        public async Task SetAlarmActionTests_FailOnWrongTimeFormat(string time)
        {
            var expected = Core.Domain.Models.Enums.ErrorMessageType.WrongTimeFormat;
            var got = await new SetAlarmAction(CreateMockService()).Do(
                new Message()
                {
                    Chat = new Chat() { Id = UserStateContextFactory.UsersID.Values.Max() + 1 },
                    Text = time,
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.Null(got.Result);
            Assert.Equal(expected, got.Error);
        }
    }
}
