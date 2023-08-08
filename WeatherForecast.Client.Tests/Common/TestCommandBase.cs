using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Persistence;

namespace WeatherForecast.Client.Tests.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly UserStateDbContext Context;

        public TestCommandBase()
        {
            Context = UserStateContextFactory.Create();
        }

        public void Dispose()
        {
            UserStateContextFactory.Destroy(Context);
        }
    }
}
