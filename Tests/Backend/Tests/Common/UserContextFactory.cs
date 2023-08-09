using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Persistence;

namespace WeatherForecast.Backend.Tests.Common
{
    public class UserContextFactory
    {
        public static readonly IReadOnlyDictionary<string, long> UsersID = new Dictionary<string, long>()
        {
            ["UserIdA"] = 1,
            ["UserIdB"] = 2,
            ["UserIdForUpdate"] = 3,
            ["UserIdForDelete"] = 4,
        };

        public static UserDbContext Create()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new UserDbContext(options);
            context.Database.EnsureCreated();
            context.Users.AddRange(
                new Core.Domain.User
                {
                    Id = UsersID["UserIdA"],
                    Latitude = 10.56,
                    Longitude = 34.57,
                },
                new Core.Domain.User
                {
                    Id = UsersID["UserIdB"],
                    Latitude = 1.5723,
                    Longitude = 43.57,
                },
                new Core.Domain.User
                {
                    Id = UsersID["UserIdForUpdate"],
                    Latitude = 57.42,
                    Longitude = 29.31,
                },
                new Core.Domain.User
                {
                    Id = UsersID["UserIdForDelete"],
                    Latitude = 19.42,
                    Longitude = 37.656,
                }
            );
            context.SaveChanges();
            return context;
        }

        public static void Destroy(UserDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
