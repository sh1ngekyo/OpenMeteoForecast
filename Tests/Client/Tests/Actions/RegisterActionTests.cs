﻿using MediatR;


using NSubstitute;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;

using UserRemoteApi;
using UserRemoteApi.Commands.RegisterUser;
using UserRemoteApi.Interfaces;

using WeatherForecast.Client.Core.Application.States.Actions;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.Actions
{
    public class RegisterActionTests
    {

        [Fact]
        public async Task RegisterActionTests_Success()
        {
            var _userManageService = Substitute.For<IUserManageService>();
            _userManageService.HandleAsync(Arg.Any<RegisterUserCommand>(), Arg.Any<CancellationToken>())
                .Returns(
                new Response<Unit>() 
                {
                    Result = new Unit() 
                });
            var got = await new RegisterAction(_userManageService).Do(
                new Message()
                {
                    Chat = new Chat() { Id = UserStateContextFactory.UsersID.Values.Max() + 1 },
                    Location = new Location() { Latitude = 11, Longitude = 11 }
                }, CancellationToken.None);
            Assert.NotNull(got);
            Assert.NotNull(got.Result);
            Assert.True(got.Error == Core.Domain.Models.Enums.ErrorMessageType.None);
            Assert.Equal(Core.Domain.Models.Enums.MessageType.RegisterOk, got.Result!.Response);
        }
    }
}