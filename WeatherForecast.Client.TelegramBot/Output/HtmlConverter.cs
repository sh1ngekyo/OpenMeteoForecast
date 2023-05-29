using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.TelegramBot.Interfaces;
using WeatherForecast.Client.Core.Application.Extensions;

namespace WeatherForecast.Client.TelegramBot.Output
{
    public class HtmlConverter : IAttachmentConverter
    {
        private readonly string _fileSkeleton;

        public HtmlConverter(string skeleton)
        {
            _fileSkeleton = skeleton;
        }

        private string GetImageAsset(WeatherCode code, string fileName)
        {
            var path = Path.Combine(@"..\..\..\",
                $@"Content\Assets\{(int)code}\{fileName}.svg");
            var img = $"<img ";
            if (code < WeatherCode.PartlyCloudySky && fileName == "night")
                img += "style=\"right:25%;\"";
            return $"{img} src=\"{path}\">";
        }

        private async Task<string> ConvertDay(FullDayForecastAttachment attachment)
        {
            var builder = new StringBuilder();
            builder.Append(@"
                <tr>
                <th>Время</th>
                <th>t°</th>
                <th>Погода</th>
                <th>Время</th>
                <th>t°</th>
                <th>Погода</th>
                </tr>");
            for (int i = 0; i < attachment.DailyForecast.Length / 2; i++)
            {
                builder.Append(@$"
                    <tr>
                        <td>{attachment.DailyForecast[i].Time.ToShortTimeString()}</td>
                        <td>{attachment.DailyForecast[i].Temperature}</td>
                        <td>{GetImageAsset(attachment.DailyForecast[i].WeatherCode, attachment.DailyForecast[i].Time.Hour > 6 ? "day" : "night")}</td>
                        <td>{attachment.DailyForecast[i + 12].Time.ToShortTimeString()}</td>
                        <td>{attachment.DailyForecast[i + 12].Temperature}</td>
                        <td>{GetImageAsset(attachment.DailyForecast[i + 12].WeatherCode, attachment.DailyForecast[i + 12].Time.Hour < 21 ? "day" : "night")}</td>
                    </tr>");
            }
            var converter = new ImageGenerator();
            var bytes = converter.FromHtmlString(_fileSkeleton.Insert(_fileSkeleton.IndexOf("</table>"), builder.ToString()));
            var path = Guid.NewGuid().ToString();
            await System.IO.File.WriteAllBytesAsync(path, bytes);
            return path;
        }

        private async Task<string> ConvertWeek(WeekForecastAttachment attachment)
        {
            var builder = new StringBuilder();
            builder.Append(@"
                <tr>
                <th>День</th>
                <th>t°</th>
                <th>Погода</th>
                </tr>");
            for (int i = 0; i < attachment.WeeklyForecast.Length; i++)
            {
                builder.Append(@$"
                    <tr>
                        <td>{attachment.WeeklyForecast[i].Day.DayOfWeek}</td>
                        <td>{attachment.WeeklyForecast[i].AvgTemperature}</td>
                        <td>{GetImageAsset(attachment.WeeklyForecast[i].WeatherCode, "day")}</td>
                    </tr>");
            }
            var converter = new ImageGenerator();
            var bytes = converter.FromHtmlString(_fileSkeleton.Insert(_fileSkeleton.IndexOf("</table>"), builder.ToString()));
            var path = Guid.NewGuid().ToString();
            await System.IO.File.WriteAllBytesAsync(path, bytes);
            return path;
        }

        private async Task<string> ConvertNow(NowForecastAttachment attachment)
        {
            var builder = new StringBuilder();
            builder.Append(@$"
            <tr>
                <td>Погода</td>
                <td>{GetImageAsset(attachment.WeatherCode, (attachment.Time.Hour < 6 || attachment.Time.Hour > 20) ? "night" : "day")}
                {attachment.WeatherCode.FromCodeToString()}
                </td>
            </tr>
            <tr>
                <td>t°</td>
                <td>{attachment.Temperature}</td>
            </tr>
            <tr>
                <td>Время</td>
                <td>{attachment.Time.ToShortTimeString()}</td>
            </tr>");
            var converter = new ImageGenerator();
            var html = _fileSkeleton.Insert(_fileSkeleton.IndexOf("</table>"), builder.ToString());
            html.Replace("<body>", "<body style=\"width:300px;\">");
            var bytes = converter.FromHtmlString(html.Replace("<body>", "<body style=\"width:450px;\">"), 450);
            var path = Guid.NewGuid().ToString();
            await System.IO.File.WriteAllBytesAsync(path, bytes);
            return path;
        }

        public async Task<string> Convert(Attachment attachment)
        => attachment switch
        {
            WeekForecastAttachment => await ConvertWeek(attachment as WeekForecastAttachment),
            FullDayForecastAttachment => await ConvertDay(attachment as FullDayForecastAttachment),
            _ => await ConvertNow(attachment as NowForecastAttachment)
        };
    }
}
