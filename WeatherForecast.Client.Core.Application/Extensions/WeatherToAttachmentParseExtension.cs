using OpenMeteoRemoteApi.Models;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.Extensions
{
    public static class WeatherToAttachmentParseExtension
    {
        private const int HoursInDay = 24;
        private const int DaysInWeek = 7;

        private static Attachment ParseAsNow(WeatherResponse weatherResponse)
        {
            return new NowForecastAttachment()
            {
                Temperature = weatherResponse.CurrentWeather!.Temperature!.Value,
                Time = DateTime.ParseExact(
                    weatherResponse.CurrentWeather!.Time!,
                    "yyyy-MM-ddTHH:mm",
                    CultureInfo.InvariantCulture),
                WeatherCode = weatherResponse.CurrentWeather!.WeatherCode,
            };
        }

        private static Attachment ParseAsDay(WeatherResponse weatherResponse, int day)
        {
            var result = new FullDayForecastAttachment();
            result.DailyForecast = new (DateTime Time, double Temperature, WeatherCode WeatherCode)[HoursInDay];
            for (int i = day * HoursInDay; i < day * HoursInDay + HoursInDay; i++)
            {
                result.DailyForecast[i % HoursInDay] = new(
                    DateTime
                    .ParseExact(
                        weatherResponse.HourlyWeather!.Time![i],
                        "yyyy-MM-ddTHH:mm",
                        CultureInfo.InvariantCulture),
                    weatherResponse.HourlyWeather!.Temperature![i],
                    weatherResponse.HourlyWeather!.WeatherCode![i]!);
            }
            return result;
        }

        private static Attachment ParseAsWeek(WeatherResponse weatherResponse)
        {
            var result = new WeekForecastAttachment();
            result.WeeklyForecast = new (DateTime Time, double Temperature, WeatherCode WeatherCode)[DaysInWeek];
            for (int i = 0; i < DaysInWeek; i++)
            {
                result.WeeklyForecast[i] = new(
                    DateTime
                    .ParseExact(
                        weatherResponse.HourlyWeather!.Time!.Skip(i * HoursInDay).First(),
                        "yyyy-MM-ddTHH:mm",
                        CultureInfo.InvariantCulture),
                    Math.Round(
                        weatherResponse.HourlyWeather!.Temperature!
                        .Skip(i * HoursInDay)
                        .Take(HoursInDay)
                        .Average(), 1),
                    weatherResponse.HourlyWeather!.WeatherCode!
                    .Skip(i * HoursInDay)
                    .Take(HoursInDay)
                    .Min());
            }
            return result;
        }

        public static OutputMessage Parse(this WeatherResponse weatherResponse, ForecastRequest forecastRequest) =>
            forecastRequest switch
            {
                ForecastRequest.Day => new OutputMessage()
                {
                    Response = MessageType.ForecastToday,
                    Attachments = new List<Attachment>() { ParseAsDay(weatherResponse, 0) }
                },
                ForecastRequest.Next => new OutputMessage()
                {
                    Response = MessageType.ForecastNext,
                    Attachments = new List<Attachment>() { ParseAsDay(weatherResponse, 1) }
                },
                ForecastRequest.Week => new OutputMessage()
                {
                    Response = MessageType.ForecastWeek,
                    Attachments = new List<Attachment>() { ParseAsWeek(weatherResponse) }
                },
                _ => new OutputMessage()
                {
                    Response = MessageType.ForecastNow,
                    Attachments = new List<Attachment>() { ParseAsNow(weatherResponse) }
                },
            };
    }
}
