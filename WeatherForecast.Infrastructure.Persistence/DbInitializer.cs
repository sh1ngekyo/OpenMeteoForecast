using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Infrastructure.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(UserDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
