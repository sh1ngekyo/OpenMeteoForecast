﻿using MediatR;
using Moq;
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
using UserRemoteApi.Queries.GetUser;
using UserRemoteApi.Models;
using UserRemoteApi.Commands.UpdateUser;

namespace WeatherForecast.Client.Tests.States
{
    public class WaitingForTimeIntervalTests
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

            return (new WaitingForTimeInterval(userManageService), ctx);
        }

        [Fact]
        public async Task WaitingForTimeIntervalTests_Success()
        {
            var expectedError = ErrorMessageType.None;
            var expectedResult = MessageType.WarningsOn;

            var userManageServiceSetupBehavior = () =>
            {
                var _userManageService = new Mock<IUserManageService>();
                _userManageService.Setup(
                    _ => _.HandleAsync(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                    new Response<UserViewModel?>()
                    {
                        Result = new UserViewModel() { Id = 1 },
                    });
                _userManageService.Setup(
                    _ => _.HandleAsync(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                    new Response<Unit>()
                    {
                        Result = new Unit()
                    });
                return _userManageService.Object;
            };
            var requestedObjects = SetupRequestedObjects(userManageServiceSetupBehavior, StateType.WaitingForTimeSpan);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message()
            {
                Chat = new Telegram.Bot.Types.Chat()
                {
                    Id = 1,
                },
                Text = "12:00-17:00"
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

            var requestedObjects = SetupRequestedObjects(() => null, StateType.WaitingForTimeInterval);
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
