using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;

namespace WeatherForecast.Client.Persistence
{
    public static class DI
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<UserStateDbContext>(opt =>
            {
                opt.UseSqlite(connectionString);
            });
            services.AddTransient<IUserStateDbContext>(provider => provider.GetService<UserStateDbContext>()!);
            return services;
        }
    }
}
