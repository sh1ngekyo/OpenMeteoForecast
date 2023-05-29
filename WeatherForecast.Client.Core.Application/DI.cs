using HttpService;
using Microsoft.Extensions.DependencyInjection;

using OpenMeteoRemoteApi;
using OpenMeteoRemoteApi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi;
using UserRemoteApi.Interfaces;
using WeatherForecast.Client.Core.Application.Interfaces;

using WeatherForecast.Client.Core.Application.Notifications;
using WeatherForecast.Client.Core.Application.States;
using WeatherForecast.Client.Core.Application.States.Factories;

namespace WeatherForecast.Client.Core.Application
{
    public static class DI
    {
        private static T ResolveWith<T>(this IServiceProvider provider, params object[] parameters) where T : class =>
            ActivatorUtilities.CreateInstance<T>(provider, parameters);

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(UserManageService).Assembly));
            services.AddSingleton<IUserManageService, UserManageService>();
            services.AddSingleton<IWeatherApiWrapper>(p => p.ResolveWith<OpenMeteoApiWrapper>(new HttpService.HttpService($"https://api.open-meteo.com/")));
            services.AddTransient<IHttpService>(p => p.ResolveWith<HttpService.HttpService>("http://localhost:5000"));
            services.AddSingleton<WaitingForLocationFactory>();
            services.AddSingleton<WaitingForNewMessagesFactory>();
            services.AddSingleton<WaitingForTimeSpanFactory>();
            services.AddSingleton<WaitingForTimeIntervalFactory>();
            services.AddSingleton<IStateStrategy>(p => p.ResolveWith<StateStrategy>(
                new object[]
                {
                    new IStateFactory[]
                    {
                        p.GetRequiredService<WaitingForNewMessagesFactory>(),
                        p.GetRequiredService<WaitingForLocationFactory>(),
                        p.GetRequiredService<WaitingForTimeSpanFactory>(),
                        p.GetRequiredService<WaitingForTimeIntervalFactory>(),
                    }
                })
            );
            services.AddSingleton<WarningsNotificationService>();
            services.AddSingleton<AlarmNotificationService>();
            services.AddSingleton<IUserManager, UserManager>();
            return services;
        }
    }
}
