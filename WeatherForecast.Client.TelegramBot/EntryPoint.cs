using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

using WeatherForecast.Client.Core.Application;
using WeatherForecast.Client.Core.Application.Notifications;
using WeatherForecast.Client.Persistence;
using WeatherForecast.Client.TelegramBot.Interfaces;

using WeatherForecast.Client.TelegramBot.Output;

using WeatherForecast.Client.TelegramBot.Telegram;

namespace WeatherForecast.Client.TelegramBot
{
    public static class ServiceProviderExtensions
    {
        public static T ResolveWith<T>(this IServiceProvider provider, params object[] parameters) where T : class =>
            ActivatorUtilities.CreateInstance<T>(provider, parameters);
    }
    public class Program
    {

        static async Task Main(string[] args)
        {
            var content = await LoadContent();
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appconfig.json", optional: false);
            IConfiguration configuration = builder.Build();
            var serviceProvider = CreateServiceProvider();
            ServiceProvider CreateServiceProvider()
            {
                var collection = new ServiceCollection();
                collection.AddSingleton<IAttachmentConverter>(new HtmlConverter(content));
                collection.AddSingleton(configuration);
                collection.AddSingleton<MessageSender>();
                collection.AddSingleton<ITelegramBotClient>(p => p.ResolveWith<TelegramBotClient>(configuration.GetRequiredSection("TelegramToken").Value));
                collection.AddApplication();
                collection.AddPersistence(configuration);
                collection.AddSingleton<TelegramClient>();
                return collection.BuildServiceProvider();
            }
            var context = serviceProvider.GetRequiredService<UserStateDbContext>();
            DbInitializer.Initialize(context);

            using CancellationTokenSource cts = new();
            Console.WriteLine($"Start listening for @{await serviceProvider.GetRequiredService<TelegramClient>().StartAsync(serviceProvider.GetRequiredService<ITelegramBotClient>(), cts.Token)}");

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService(p =>
                    p.ResolveWith<NotificationBackgroundService>(
                        serviceProvider.GetRequiredService<TelegramClient>(),
                        serviceProvider.GetRequiredService<AlarmNotificationService>(),
                        serviceProvider.GetRequiredService<WarningsNotificationService>()));
                })
                .Build();
            host.Run();

            Console.ReadLine();
            cts.Cancel();
        }
        private static async Task<string> LoadContent()
        {
            var path = Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
                @"Content\Table.html");
            return await System.IO.File.ReadAllTextAsync(path);
        }
    }
}
