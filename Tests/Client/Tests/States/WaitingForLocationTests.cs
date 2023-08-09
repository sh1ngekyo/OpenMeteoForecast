using MediatR;

using NSubstitute;

using OpenMeteoRemoteApi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Commands.RegisterUser;
using UserRemoteApi.Commands.UpdateUser;
using UserRemoteApi.Interfaces;
using UserRemoteApi.Models;
using UserRemoteApi.Queries.GetUser;

using WeatherForecast.Client.Core.Application.Context;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Application.States;
using WeatherForecast.Client.Core.Application.States.Factories;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Tests.States
{
    public class WaitingForLocationTests
    {
        private readonly Telegram.Bot.Types.Message _requestMessage = new Telegram.Bot.Types.Message()
        {
            Chat = new Telegram.Bot.Types.Chat()
            {
                Id = 1,
            },
            Location = new Telegram.Bot.Types.Location()
            {
                Latitude = 11,
                Longitude = 12
            }
        };

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

            return (new WaitingForLocation(userManageService), ctx);
        }

        [Fact]
        public async Task WaitingForLocationTests_SuccessRegister()
        {
            var expectedError = ErrorMessageType.None;
            var expectedResult = MessageType.RegisterOk;

            var userManageServiceSetupBehavior = () =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<UserViewModel?>()
                    {
                        Error = Core.Domain.Models.Enums.ErrorMessageType.NotFound
                    });
                _userManageService.HandleAsync(Arg.Any<RegisterUserCommand>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<Unit>()
                    {
                        Result = new Unit()
                    });
                return _userManageService;
            };
            var requestedObjects = SetupRequestedObjects(userManageServiceSetupBehavior, StateType.WaitingForLocation);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, _requestMessage);

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedError, response.Error);
            Assert.Equal(expectedResult, response.Result.Response);
        }

        [Fact]
        public async Task WaitingForLocationTests_FailRegisterWithError()
        {
            var expectedError = ErrorMessageType.InternalServerError;

            var userManageServiceSetupBehavior = () =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<UserViewModel?>()
                    {
                        Error = Core.Domain.Models.Enums.ErrorMessageType.InternalServerError
                    });
                return _userManageService;
            };

            var requestedObjects = SetupRequestedObjects(userManageServiceSetupBehavior, StateType.WaitingForLocation);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, _requestMessage);

            Assert.NotNull(response);
            Assert.Null(response.Result);
            Assert.Equal(expectedError, response.Error);
        }

        [Fact]
        public async Task WaitingForLocationTests_FailRegisterLocationIsNull()
        {
            var expectedError = ErrorMessageType.UnknownMessageType;

            var userManageServiceSetupBehavior = () =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<UserViewModel?>()
                    {
                        Error = Core.Domain.Models.Enums.ErrorMessageType.InternalServerError
                    });
                return _userManageService;
            };

            var requestedObjects = SetupRequestedObjects(userManageServiceSetupBehavior, StateType.WaitingForLocation);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, new Telegram.Bot.Types.Message()
            {
                Chat = new Telegram.Bot.Types.Chat()
                {
                    Id = 1
                }
            });

            Assert.NotNull(response);
            Assert.Null(response.Result);
            Assert.Equal(expectedError, response.Error);
        }

        [Fact]
        public async Task WaitingForLocationTests_UpdateIfUserExists()
        {
            var expectedError = ErrorMessageType.None;
            var expectedResult = MessageType.UpdateOk;

            var userManageServiceSetupBehavior = () =>
            {
                var _userManageService = Substitute.For<IUserManageService>();
                _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<UserViewModel?>()
                    {
                        Result = new UserViewModel() { Id = 1 }
                    });
                _userManageService.HandleAsync(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>())
                    .Returns(
                    new Response<Unit>()
                    {
                        Result = new Unit()
                    });
                return _userManageService;
            };

            var requestedObjects = SetupRequestedObjects(userManageServiceSetupBehavior, StateType.WaitingForLocation);
            var response = await requestedObjects.State.Handle(requestedObjects.Context, _requestMessage);

            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedError, response.Error);
            Assert.Equal(expectedResult, response.Result.Response);
        }
    }
}
