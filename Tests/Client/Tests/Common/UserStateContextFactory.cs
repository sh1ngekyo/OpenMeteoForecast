using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Persistence;

namespace WeatherForecast.Client.Tests.Common
{
    public class UserStateContextFactory
    {
        public static readonly IReadOnlyDictionary<string, long> UsersID = new Dictionary<string, long>()
        {
            ["WaitingForNewMessagesUserId"] = 1,
            ["WaitingForLocationUserId"] = 2,
            ["WaitingForTimeIntervalUserId"] = 3,
            ["WaitingForTimeSpanUserId"] = 4,
            ["UserIdForUpdate"] = 5,
            ["UserIdForDelete"] = 6,
        };

        public static UserStateDbContext Create()
        {
            var options = new DbContextOptionsBuilder<UserStateDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new UserStateDbContext(options);
            context.Database.EnsureCreated();
            context.UsersStates.AddRange(
                new Core.Domain.Models.UserState
                {
                    UserId = UsersID["WaitingForNewMessagesUserId"],
                    StateType = Core.Domain.Models.Enums.StateType.WaitingForNewMessages
                },
                new Core.Domain.Models.UserState
                {
                    UserId = UsersID["WaitingForLocationUserId"],
                    StateType = Core.Domain.Models.Enums.StateType.WaitingForLocation
                }, 
                new Core.Domain.Models.UserState
                {
                    UserId = UsersID["WaitingForTimeIntervalUserId"],
                    StateType = Core.Domain.Models.Enums.StateType.WaitingForTimeInterval
                },
                new Core.Domain.Models.UserState
                {
                    UserId = UsersID["WaitingForTimeSpanUserId"],
                    StateType = Core.Domain.Models.Enums.StateType.WaitingForTimeSpan
                },
                new Core.Domain.Models.UserState
                {
                    UserId = UsersID["UserIdForUpdate"],
                    StateType = Core.Domain.Models.Enums.StateType.WaitingForNewMessages
                },
                new Core.Domain.Models.UserState
                {
                    UserId = UsersID["UserIdForDelete"],
                    StateType = Core.Domain.Models.Enums.StateType.WaitingForNewMessages
                }
            ) ;
            context.SaveChanges();
            return context;
        }

        public static void Destroy(UserStateDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
