using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.States.Commands.CreateUserState;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.States.Commands
{
    public class CreateUserStateCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateUserCommandHandler_Success()
        {
            var handler = new CreateUserStateCommandHandler(Context);

            var state = await handler.Handle(
                new CreateUserStateCommand
                {
                    Id = UserStateContextFactory.UsersID.Values.Max() + 1
                },
                CancellationToken.None);

            Assert.NotNull(
                await Context.UsersStates.SingleOrDefaultAsync(st =>
                    st.UserId == state.UserId && st.StateType == state.StateType));

            Assert.Equal(StateType.WaitingForNewMessages, state.StateType);
        }

        [Fact]
        public async Task CreateUserCommandHandler_ShouldReturnStateIfUserExists()
        {
            var handler = new CreateUserStateCommandHandler(Context);

            var id = UserStateContextFactory.UsersID["WaitingForLocationUserId"];
            var type = StateType.WaitingForLocation;

            var state = await handler.Handle(
                new CreateUserStateCommand
                {
                    Id = id
                },
                CancellationToken.None);

            Assert.Equal(id, state.UserId);
            Assert.Equal(type, state.StateType);
        }
    }
}
