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
    public class EnableWarningsActionTests
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
        [InlineData("12:35-23:00")]
        [InlineData("07:00-09:30")]
        [InlineData("15:00-15:30")]
        public async Task EnableWarningsActionTests_Success(string time)
        {
            var expected = Core.Domain.Models.Enums.MessageType.WarningsOn;
            var got = await new EnableWarningsAction(CreateMockService()).Do(
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
        [InlineData("2:35-3:00")]
        [InlineData("7:00-9:30")]
        [InlineData("5:00-5:30")]
        public async Task EnableWarningsActionTests_SuccessWithPadLeft(string time)
        {
            var expected = Core.Domain.Models.Enums.MessageType.WarningsOn;
            var got = await new EnableWarningsAction(CreateMockService()).Do(
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
        [InlineData("2:35 3:00")]
        [InlineData("7:00:9:30")]
        [InlineData("500")]
        [InlineData("test")]
        public async Task EnableWarningsActionTests_FailOnWrongTimeFormat(string time)
        {
            var expected = Core.Domain.Models.Enums.ErrorMessageType.WrongTimeFormat;
            var got = await new EnableWarningsAction(CreateMockService()).Do(
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
