using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Client.Core.Application.States.Commands.CreateUserState;
using WeatherForecast.Client.Core.Application.States.Commands.DeleteUserState;
using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.States.Commands
{
    public class DeleteUserStateCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteUserStateCommandHandler_Success()
        {
            var handler = new DeleteUserStateCommandHandler(Context);

            await handler.Handle(new DeleteUserStateCommand
            {
                Id = UserStateContextFactory.UsersID["UserIdForDelete"],
            }, CancellationToken.None);

            Assert.Null(Context.UsersStates.SingleOrDefault(state =>
                state.UserId == UserStateContextFactory.UsersID["UserIdForDelete"]));
        }
    }
}
