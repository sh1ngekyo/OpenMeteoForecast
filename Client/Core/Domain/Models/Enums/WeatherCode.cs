using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Domain.Models.Enums
{
    public enum WeatherCode
    {
        ClearSky = 0,
        MainlyClearSky = 1,
        PartlyCloudySky = 2,
        OvercastSky = 3,
        Fog = 45,
        DepositingRimeFog = 48,
        DrizzleLight = 51,
        DrizzleModerate = 53,
        DrizzleDenseIntensity = 55,
        FreezingDrizzleLight = 56,
        FreezingDrizzleDenseIntensity = 57,
        RainSlight = 61,
        RainModerate = 63,
        RainHeavyIntensity = 65,
        FreezingRainLight = 66,
        FreezingRainHeavyIntensity = 67,
        SnowfallSlight = 71,
        SnowfallModerate = 73,
        SnowfallHeavyIntensity = 75,
        SnowGrains = 77,
        RainShowersSlight = 80,
        RainShowersModerate = 81,
        RainShowersViolent = 82,
        SnowShowersSlight = 85,
        SnowShowersHeavy = 86,
        ThunderstormSlightOrModerate = 95,
        ThunderstormWithSlightHail = 96,
        ThunderstormWithSlightHeavyHail = 99
    }
}
