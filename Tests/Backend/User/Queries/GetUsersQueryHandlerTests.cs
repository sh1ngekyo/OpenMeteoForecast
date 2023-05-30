using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Backend.Tests.Common;
using WeatherForecast.Backend.Core.Application.Common.Exceptions;
using WeatherForecast.Backend.Core.Application.Users.Queries.GetUsers;
using WeatherForecast.Backend.Core.Application.Users.Queries;

using WeatherForecast.Infrastructure.Persistence;

namespace WeatherForecast.Backend.Tests.User.Queries
{
    [Collection("QueryCollection")]
    public class GetUsersQueryHandlerTests
    {
        private readonly UserDbContext Context;
        private readonly IMapper Mapper;

        public GetUsersQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetUserQueryHandler_Success()
        {
            var handler = new GetUserListQueryHandler(Context, Mapper);
            var count = Context.Users.Count();
            var result = await handler.Handle(
                new GetUserListQuery(),
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<UserListViewModel>(result);
            Assert.Equal(count, result.Users.Count);
            foreach (var user in result.Users)
            {
                Assert.NotNull(user);
            }
        }
    }
}
