using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRemoteApi.Interfaces;
using UserRemoteApi.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Application.States;
using UserRemoteApi.Commands.RegisterUser;
using OpenMeteoRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Context;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Application.States.Factories;
using OpenMeteoRemoteApi.Models;
using Telegram.Bot.Types;
using WeatherForecast.Client.Core.Application.States.Actions;
using UserRemoteApi.Queries.GetUser;
using UserRemoteApi.Commands.UpdateUser;
using NSubstitute;

namespace WeatherForecast.Client.Tests.States
{
    public class WaitingForNewMessagesTests
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

            return (new WaitingForNewMessages(userManageService, weatherApi), ctx);
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_ShouldHandleOnlyText()
        {
            var expectedNoTextInputResult = ErrorMessageType.UnknownMessageType;
            var expectedTextInputResult = ErrorMessageType.UnknownCommandType;

            var response = await SetupRequestedObjects(() => default!, StateType.WaitingForNewMessages).State.Handle(default!, new Telegram.Bot.Types.Message
            {
                Video = new Telegram.Bot.Types.Video()
            });

            Assert.NotNull(response);
            Assert.Null(response.Result);
            Assert.Equal(expectedNoTextInputResult, response.Error);

            response = await SetupRequestedObjects(() => default!, StateType.WaitingForNewMessages).State.Handle(default!, new Telegram.Bot.Types.Message
            {
                Text = "Test"
            });
            Assert.NotNull(response);
            Assert.Null(response.Result);
            Assert.Equal(expectedTextInputResult, response.Error);
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_ShouldRequestLocationFromRegisterCommand()
        {
            var expectedResult = MessageType.LocationRequest;
            var requestedObjects = SetupRequestedObjects(() => default!, StateType.WaitingForNewMessages);

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Text = "/register"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForLocation, requestedObjects.Context.GetStateType());
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_ShouldRequestLocationFromUpdateCommand()
        {
            var expectedResult = MessageType.LocationRequest;
            var requestedObjects = SetupRequestedObjects(() => default!, StateType.WaitingForNewMessages);

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Text = "/update"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForLocation, requestedObjects.Context.GetStateType());
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_ShouldRequestTimeSpanFromAlarmOnCommand()
        {
            var expectedResult = MessageType.TimeSpanRequest;
            var requestedObjects = SetupRequestedObjects(() => default!, StateType.WaitingForNewMessages);

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Text = "/alarm_on"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForTimeSpan, requestedObjects.Context.GetStateType());
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_ShouldRequestTimeIntervalFromWarningsOnCommand()
        {
            var expectedResult = MessageType.TimeIntervalRequest;
            var requestedObjects = SetupRequestedObjects(() => default!, StateType.WaitingForNewMessages);

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Text = "/warnings_on"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForTimeInterval, requestedObjects.Context.GetStateType());
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_SuccessOnHandleForecastCommand()
        {
            var expectedResult = MessageType.ForecastNow;

            var _userInfo = new UserViewModel()
            {
                Id = 1,
                Latitude = 55.7558,
                Longitude = 37.618423
            };

            var requestedObjects = SetupRequestedObjects(() =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<UserViewModel?>()
                    {
                        Result = _userInfo
                    });
                return _userManageService;
            }, StateType.WaitingForNewMessages,
            () =>
            {
                var _weatherApi = Substitute.For<IWeatherApiWrapper>();
                _weatherApi.GetWeatherAsync(Arg.Any<WeatherRequest>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<WeatherResponse?>()
                    {
                        Result = new WeatherResponse()
                        {
                            CurrentWeather = new CurrentWeather()
                            {
                                Time = "2000-01-01T12:00",
                                Temperature = 0,
                                WeatherCode = Core.Domain.Models.Enums.WeatherCode.ClearSky
                            }
                        }
                    });
                return _weatherApi;
            });

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Chat = new Chat() { Id = _userInfo.Id },
                Text = "/forecast"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForNewMessages, requestedObjects.Context.GetStateType());
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_SuccessOnHandleAlarmOffCommand()
        {
            var expectedResult = MessageType.AlarmOff;

            var _userInfo = new UserViewModel()
            {
                Id = 1
            };

            var requestedObjects = SetupRequestedObjects(() =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<Unit>()
                    {
                        Result = new Unit()
                    });
                return _userManageService;
            }, StateType.WaitingForNewMessages);

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Chat = new Chat() { Id = _userInfo.Id },
                Text = "/alarm_off"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForNewMessages, requestedObjects.Context.GetStateType());
        }

        [Fact]
        public async Task WaitingForNewMessagesTests_SuccessOnHandleWarningsOffCommand()
        {
            var expectedResult = MessageType.WarningsOff;

            var _userInfo = new UserViewModel()
            {
                Id = 1
            };

            var requestedObjects = SetupRequestedObjects(() =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<Unit>()
                    {
                        Result = new Unit()
                    });
                return _userManageService;
            }, StateType.WaitingForNewMessages);

            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message
            {
                Chat = new Chat() { Id = _userInfo.Id },
                Text = "/warnings_off"
            });

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result.Response);
            Assert.Equal(StateType.WaitingForNewMessages, requestedObjects.Context.GetStateType());
        }
    }
}
