using MediatR;
using Moq;

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

            var _userManageService = new Mock<IUserManageService>();
            _userManageService.Setup(
                _ => _.HandleAsync(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new Response<UserViewModel?>()
                {
                    Result = _dummyUser
                });
            var _weatherApi = new Mock<IWeatherApiWrapper>();
            _weatherApi.Setup(
                _ => _.GetWeatherAsync(It.IsAny<WeatherRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
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

            var got = await new GetForecastAction(ForecastRequest.Now, _userManageService.Object, _weatherApi.Object).Do(
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

            var _userManageService = new Mock<IUserManageService>();
            _userManageService.Setup(
                _ => _.HandleAsync(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new Response<UserViewModel?>()
                {
                    Error = ErrorMessageType.NotFound,
                });

            var got = await new GetForecastAction(ForecastRequest.Now, _userManageService.Object, default!).Do(
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

            var _userManageService = new Mock<IUserManageService>();
            _userManageService.Setup(
                _ => _.HandleAsync(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new Response<UserViewModel?>()
                {
                    Result = _dummyUser
                });
            var _weatherApi = new Mock<IWeatherApiWrapper>();
            _weatherApi.Setup(
                _ => _.GetWeatherAsync(It.IsAny<WeatherRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new Response<WeatherResponse?>()
                {
                    Error = ErrorMessageType.WeatherServiceUnavaliable,
                });

            var got = await new GetForecastAction(ForecastRequest.Now, _userManageService.Object, _weatherApi.Object).Do(
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
