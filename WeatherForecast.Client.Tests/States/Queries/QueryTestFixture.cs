using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Persistence;
using WeatherForecast.Client.Tests.Common;

namespace WeatherForecast.Client.Tests.States.Queries
{
    public class QueryTestFixture : IDisposable
    {
        public UserStateDbContext Context;

        public QueryTestFixture()
        {
            Context = UserStateContextFactory.Create();
        }

        public void Dispose()
        {
            UserStateContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
