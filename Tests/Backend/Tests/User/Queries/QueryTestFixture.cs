using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Core.Application.Common.Mappings;
using WeatherForecast.Backend.Core.Application.Interfaces;
using WeatherForecast.Backend.Persistence;
using WeatherForecast.Backend.Tests.Common;


namespace WeatherForecast.Backend.Tests.User.Queries
{
    public class QueryTestFixture : IDisposable
    {
        public UserDbContext Context;
        public IMapper Mapper;

        public QueryTestFixture()
        {
            Context = UserContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(IUserDbContext).Assembly));
            });
            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            UserContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
