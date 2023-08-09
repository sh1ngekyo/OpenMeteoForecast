using MediatR;
using OpenMeteoRemoteApi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Context;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Application.States.Factories;
using WeatherForecast.Client.Core.Application.States;

using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models;
using Xunit;
using UserRemoteApi.Queries.GetUser;
using UserRemoteApi.Models;
using UserRemoteApi.Commands.UpdateUser;
using NSubstitute;

namespace WeatherForecast.Client.Tests.States
{
    public class WaitingForTimeSpanTests
    {

        private (IState State, IUserContext Context) SetupRequestedObjects(Func<IUserManageService> userManageServiceSetupBehavior, StateType stateType, Func<IWeatherApiWrapper> weatherApiSetupBehavior = null)
        {
            var userManageService = userManageServiceSetupBehavior();
            var weatherApi = weatherApiSetupBehavior is not null ? weatherApiSetupBehavior() : null;
            var ctx = new UserContext(new StateStrategy(new IStateFactory[]
            {
                new WaitingForLocationFactory(userManageService),
                new WaitingForNewMessagesFactory(userManageService, weatherApi),
                new WaitingForTimeSpanFactory(userManageService),
                new WaitingForTimeIntervalFactory(userManageService),
            }), stateType);

            return (new WaitingForTimeSpan(userManageService), ctx);
        }

        [Fact]
        public async Task WaitingForTimeSpanTests_Success()
        {
            var expectedError = ErrorMessageType.None;
            var expectedResult = MessageType.TimeSpanRequestOk;

            var userManageServiceSetupBehavior = () =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<UserViewModel?>()
                    {
                        Result = new UserViewModel() { Id = 1 },
                    });
                _userManageService.HandleAsync(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<Unit>()
                    {
                        Result = new Unit()
                    });
                return _userManageService;
            };
            var requestedObjects = SetupRequestedObjects(userManageServiceSetupBehavior, StateType.WaitingForTimeSpan);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message()
            {
                Chat = new Telegram.Bot.Types.Chat()
                {
                    Id = 1,
                },
                Text = "12:00"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedError, response.Error);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForNewMessages, requestedObjects.Context.GetStateType());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WaitingForTimeSpanTests_FailTextIsNullOrEmpty(string text)
        {
            var expectedError = ErrorMessageType.UnknownMessageType;

            var requestedObjects = SetupRequestedObjects(() => null, StateType.WaitingForTimeSpan);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message()
            {
                Chat = new Telegram.Bot.Types.Chat()
                {
                    Id = 1,
                },
                Text = text
            });

            Assert.NotNull(response);
            Assert.Null(response.Result);
            Assert.Equal(expectedError, response.Error);
            Assert.Equal(StateType.WaitingForNewMessages, requestedObjects.Context.GetStateType());
        }
    }
}
