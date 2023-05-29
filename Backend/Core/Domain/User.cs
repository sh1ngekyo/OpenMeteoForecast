namespace WeatherForecast.Backend.Core.Domain
{
    public class User
    {
        // same as telegram id
        public long Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeSpan? AlarmTime { get; set; }
        public TimeSpan? WarningsStartTime { get; set; }
        public TimeSpan? WarningsEndTime { get; set; }
    }
}