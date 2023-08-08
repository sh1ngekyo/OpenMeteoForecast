using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.Extensions
{
    public static class WeatherCodeToStringExtension
    {
        public static string FromCodeToString(this WeatherCode code)
           => (int)(code) switch
           {
               0 => "(Ясно)",
               1 => "(Преимущественно ясно)",
               2 => "(Слегка пасмурно)",
               3 => "(Пасмурно)",
               45 => "(Легкий туман)",
               48 => "(Туман, возможна изморось)",
               51 => "(Изморось)",
               53 => "(Умеренная изморось)",
               55 => "(Сильная изморось)",
               56 => "(Изморось с легким градом)",
               57 => "(Изморось с градом)",
               61 => "(Легкий дождь)",
               63 => "(Дождь)",
               65 => "(Сильный дождь)",
               66 => "(Дождь, возможен снег)",
               67 => "(Дождь и снег)",
               71 => "(Легкий снегопад)",
               73 => "(Снегопад)",
               75 => "(Сильный снегопад)",
               77 => "(Град)",
               80 => "(Легкий проливной дождь)",
               81 => "(Проливной дождь)",
               82 => "(Сильный проливной дождь)",
               85 => "(Легкий снег)",
               86 => "(Умеренный снег)",
               95 => "(Гром и гроза)",
               96 => "(Гром и гроза)",
               99 => "(Гром и гроза)",
               _ => throw new ArgumentOutOfRangeException(nameof(code)),
           };
    }
}
