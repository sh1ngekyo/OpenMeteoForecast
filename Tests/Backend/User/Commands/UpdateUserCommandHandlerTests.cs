using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Core.Application.Common.Exceptions;
using WeatherForecast.Backend.Core.Application.Users.Commands.UpdateUser;
using WeatherForecast.Backend.Tests.Common;

namespace WeatherForecast.Backend.Tests.User.Commands
{
    public class UpdateUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateUserCommandHandler_Success()
        {
            var handler = new UpdateUserCommandHandler(Context);
            var latitude = 34.567;
            var longitude = 42.568;

            await handler.Handle(new UpdateUserCommand
            {
                Id = UserContextFactory.UsersID["UserIdForUpdate"],
                Latitude = latitude,
                Longitude = longitude,
            }, CancellationToken.None);

            Assert.NotNull(await Context.Users.SingleOrDefaultAsync(User =>
                User.Id == UserContextFactory.UsersID["UserIdForUpdate"] &&
                User.Latitude == latitude && User.Longitude == longitude));
        }

        [Fact]
        public async Task UpdateUserCommandHandler_SuccessWithTime()
        {
            var handler = new UpdateUserCommandHandler(Context);

            var time = TimeSpan.FromMinutes(1);

            await handler.Handle(new UpdateUserCommand
            {
                Id = UserContextFactory.UsersID["UserIdForUpdate"],
                Alarm = new Core.Domain.Time() { Value = time },
                WarningsStartTime = new Core.Domain.Time() { Value = time },
                WarningsEndTime = new Core.Domain.Time() { Value = time },
            }, CancellationToken.None);

            Assert.NotNull(await Context.Users.SingleOrDefaultAsync(User =>
                User.Id == UserContextFactory.UsersID["UserIdForUpdate"] &&
                User.AlarmTime == time && User.WarningsStartTime == time && User.WarningsEndTime == time));
        }

        [Fact]
        public async Task UpdateUserCommandHandler_FailOnWrongId()
        {
            var handler = new UpdateUserCommandHandler(Context);

            var latitude = 34.567;
            var longitude = 42.568;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateUserCommand
                    {
                        Id = UserContextFactory.UsersID.Values.Max() + 1,
                        Latitude = latitude,
                        Longitude = longitude,
                    },
                    CancellationToken.None));
        }
    }
}
