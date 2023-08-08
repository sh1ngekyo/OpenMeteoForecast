namespace WeatherForecast.Client.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(UserStateDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}