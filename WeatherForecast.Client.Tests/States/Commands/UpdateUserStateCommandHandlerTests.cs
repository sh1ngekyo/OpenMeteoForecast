using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.States.Commands.UpdateUserState;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.States.Commands
{
    public class UpdateUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateUserStateCommandHandler_Success()
        {
            var handler = new UpdateUserStateCommandHandler(Context);

            var expected = Core.Domain.Models.Enums.StateType.WaitingForTimeInterval;
            await handler.Handle(new UpdateUserStateCommand
            {
                Id = UserStateContextFactory.UsersID["UserIdForUpdate"],
                StateType = expected
            }, CancellationToken.None);

            var user = await Context.UsersStates.SingleOrDefaultAsync(User =>
                User.UserId == UserStateContextFactory.UsersID["UserIdForUpdate"]);
            Assert.Equal(expected, user.StateType);
        }
    }
}
