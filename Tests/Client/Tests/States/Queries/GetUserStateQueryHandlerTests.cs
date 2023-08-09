using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Exceptions;
using WeatherForecast.Client.Core.Application.States.Queries.GetUserState;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Persistence;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.States.Queries
{
    [Collection("QueryCollection")]
    public class GetUserStateQueryHandlerTests
    {
        private readonly UserStateDbContext Context;

        public GetUserStateQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
        }

        [Fact]
        public async Task GetUserStateQueryHandler_Success()
        {
            var handler = new GetUserStateQueryHandler(Context);

            var result = await handler.Handle(
                new GetUserStateQuery
                {
                    Id = UserStateContextFactory.UsersID["WaitingForNewMessagesUserId"]
                },
                CancellationToken.None);

            var expected = Context.UsersStates
                .SingleOrDefault(state => state.UserId == UserStateContextFactory.UsersID["WaitingForNewMessagesUserId"]);

            Assert.NotNull(result);
            Assert.IsType<UserState>(result);

            Assert.Equal(expected!.UserId,
                result.UserId);
            Assert.Equal(expected!.StateType,
                result.StateType);
        }

        [Fact]
        public async Task GetUserStateQueryHandler_FailOnWrongId()
        {
            var handler = new GetUserStateQueryHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetUserStateQuery
                    {
                        Id = UserStateContextFactory.UsersID.Values.Max() + 1
                    },
                    CancellationToken.None));
        }
    }
}
