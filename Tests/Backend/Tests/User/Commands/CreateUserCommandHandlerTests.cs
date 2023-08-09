using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Core.Application.Common.Exceptions;
using WeatherForecast.Backend.Core.Application.Users.Commands.CreateUser;
using WeatherForecast.Backend.Tests.Common;

namespace WeatherForecast.Backend.Tests.User.Commands
{
    public class CreateUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateUserCommandHandler_Success()
        {
            var handler = new CreateUserCommandHandler(Context);
            var latitude = 34.567;
            var longitude = 42.568;

            var noteId = await handler.Handle(
                new CreateUserCommand
                {
                    Longitude = longitude,
                    Latitude = latitude,
                    Id = UserContextFactory.UsersID.Values.Max() + 1
                },
                CancellationToken.None);

            Assert.NotNull(
                await Context.Users.SingleOrDefaultAsync(note =>
                    note.Id == noteId && note.Latitude == latitude &&
                    note.Longitude == longitude));
        }

        [Fact]
        public async Task CreateUserCommandHandler_FailOnCreateExistUser()
        {
            var handler = new CreateUserCommandHandler(Context);
            var latitude = 34.567;
            var longitude = 42.568;

            await Assert.ThrowsAsync<AlreadyExistException>(async () =>
                await handler.Handle(
                    new CreateUserCommand
                    {
                        Longitude = longitude,
                        Latitude = latitude,
                        Id = UserContextFactory.UsersID.Values.FirstOrDefault()
                    },
                    CancellationToken.None));
        }
    }
}
