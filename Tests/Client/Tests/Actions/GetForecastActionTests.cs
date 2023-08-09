using MediatR;

using NSubstitute;

using OpenMeteoRemoteApi.Interfaces;
using OpenMeteoRemoteApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

using UserRemoteApi.Commands.RegisterUser;
using UserRemoteApi.Interfaces;
using UserRemoteApi.Models;
using UserRemoteApi.Queries.GetUser;

using WeatherForecast.Client.Core.Application.States.Actions;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.Actions
{
    public class GetForecastActionTests
    {
        private readonly UserViewModel _dummyUser = new UserViewModel
        {
            Id = 1,
            Latitude = 55.7558,
            Longitude = 37.618423
        };

        [Fact]
        public async Task GetForecastActionTests_SuccessForecastNow()
        {
            var expectedResult = MessageType.ForecastNow;

            var _userManageService = Substitute.For<IUserManageService>();
            _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                .Returns(
                new Response<UserViewModel?>()
                {
                    Result = _dummyUser
                });
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

            var got = await new GetForecastAction(ForecastRequest.Now, _userManageService, _weatherApi).Do(
                new Message()
                {
                    Chat = new Chat() 
                    { 
                        Id = _dummyUser.Id
                    },
                    Location = new Telegram.Bot.Types.Location() 
                    { 
                        Latitude = _dummyUser.Latitude,
                        Longitude = _dummyUser.Longitude
                    }
                }, CancellationToken.None);

            Assert.NotNull(got);
            Assert.NotNull(got.Result);
            Assert.NotNull(got.Result.Attachments);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
            Assert.Equal(expectedResult, got.Result!.Response);
        }

        [Fact]
        public async Task GetForecastActionTests_FailForecastNowWithWringUserId()
        {
            var expectedResult = ErrorMessageType.NotFound;

            var _userManageService = Substitute.For<IUserManageService>();
            _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                .Returns(
                new Response<UserViewModel?>()
                {
                    Error = ErrorMessageType.NotFound,
                });

            var got = await new GetForecastAction(ForecastRequest.Now, _userManageService, default!).Do(
                new Message()
                {
                    Chat = new Chat()
                    {
                        Id = _dummyUser.Id
                    },
                    Location = new Telegram.Bot.Types.Location()
                    {
                        Latitude = _dummyUser.Latitude,
                        Longitude = _dummyUser.Longitude
                    }
                }, CancellationToken.None);

            Assert.NotNull(got);
            Assert.Null(got.Result);
            Assert.Equal(expectedResult, got.Error);
        }

        [Fact]
        public async Task GetForecastActionTests_FailForecastNowWhenWeatherServiceUnavaliable()
        {
            var expectedResult = ErrorMessageType.WeatherServiceUnavaliable;
            var _userManageService = Substitute.For<IUserManageService>();
            _userManageService.HandleAsync(Arg.Any<GetUserQuery>(), Arg.Any<CancellationToken>())
                .Returns(
                new Response<UserViewModel?>()
                {
                    Result = _dummyUser
                });
            var _weatherApi = Substitute.For<IWeatherApiWrapper>();
            _weatherApi.GetWeatherAsync(Arg.Any<WeatherRequest>(), Arg.Any<CancellationToken>())
                .Returns(
                new Response<WeatherResponse?>()
                {
                    Error = ErrorMessageType.WeatherServiceUnavaliable,
                });

            var got = await new GetForecastAction(ForecastRequest.Now, _userManageService, _weatherApi).Do(
                new Message()
                {
                    Chat = new Chat()
                    {
                        Id = _dummyUser.Id
                    },
                    Location = new Telegram.Bot.Types.Location()
                    {
                        Latitude = _dummyUser.Latitude,
                        Longitude = _dummyUser.Longitude
                    }
                }, CancellationToken.None);

            Assert.NotNull(got);
            Assert.Null(got.Result);
            Assert.Equal(expectedResult, got.Error);
        }
    }
}
