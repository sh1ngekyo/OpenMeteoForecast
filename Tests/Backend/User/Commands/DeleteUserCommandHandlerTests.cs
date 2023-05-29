using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Core.Application.Common.Exceptions;
using WeatherForecast.Backend.Core.Application.Users.Commands.DeleteUser;
using WeatherForecast.Backend.Tests.Common;

namespace WeatherForecast.Backend.Tests.User.Commands
{
    public class DeleteUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteUserCommandHandler_Success()
        {
            var handler = new DeleteUserCommandHandler(Context);

            await handler.Handle(new DeleteUserCommand
            {
                Id = UserContextFactory.UsersID["UserIdForDelete"],
            }, CancellationToken.None);

            Assert.Null(Context.Users.SingleOrDefault(User =>
                User.Id == UserContextFactory.UsersID["UserIdForDelete"]));
        }

        [Fact]
        public async Task DeleteUserCommandHandler_FailOnWrongId()
        {
            var handler = new DeleteUserCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteUserCommand
                    {
                        Id = UserContextFactory.UsersID.Values.Max() + 1,
                    },
                    CancellationToken.None));
        }
    }
}
