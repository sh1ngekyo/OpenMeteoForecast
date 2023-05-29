using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Infrastructure.Persistence;

namespace WeatherForecast.Backend.Tests.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly UserDbContext Context;

        public TestCommandBase()
        {
            Context = UserContextFactory.Create();
        }

        public void Dispose()
        {
            UserContextFactory.Destroy(Context);
        }
    }
}
