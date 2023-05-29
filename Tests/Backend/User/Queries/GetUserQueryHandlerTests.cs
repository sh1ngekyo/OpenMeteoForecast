using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Backend.Core.Application.Common.Exceptions;
using WeatherForecast.Backend.Core.Application.Users.Queries.GetUser;
using WeatherForecast.Backend.Core.Application.Users.Queries;
using WeatherForecast.Backend.Tests.Common;

using WeatherForecast.Infrastructure.Persistence;

namespace WeatherForecast.Backend.Tests.User.Queries
{
    [Collection("QueryCollection")]
    public class GetUserQueryHandlerTests
    {
        private readonly UserDbContext Context;
        private readonly IMapper Mapper;

        public GetUserQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetUserQueryHandler_Success()
        {
            var handler = new GetUserQueryHandler(Context, Mapper);

            var result = await handler.Handle(
                new GetUserQuery
                {
                    Id = UserContextFactory.UsersID["UserIdA"]
                },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<UserViewModel>(result);
            Assert.Equal(Context.Users
                .SingleOrDefault(User => User.Id == UserContextFactory.UsersID["UserIdA"])?.Latitude,
                result.Latitude);
            Assert.Equal(Context.Users
                .SingleOrDefault(User => User.Id == UserContextFactory.UsersID["UserIdA"])?.Longitude,
                result.Longitude);
        }

        [Fact]
        public async Task GetUserQueryHandler_FailOnWrongId()
        {
            var handler = new GetUserQueryHandler(Context, Mapper);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetUserQuery
                    {
                        Id = UserContextFactory.UsersID.Values.Max() + 1
                    },
                    CancellationToken.None));
        }
    }
}
