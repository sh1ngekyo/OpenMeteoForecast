using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Core.Application.Interfaces;

namespace WeatherForecast.Infrastructure.Persistence
{
    public static class DI
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlite(connectionString);
            });
            services.AddScoped<IUserDbContext>(provider => provider.GetService<UserDbContext>()!);
            return services;
        }
    }
}
