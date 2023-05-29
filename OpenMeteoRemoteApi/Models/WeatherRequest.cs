using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMeteoRemoteApi.Models
{
    public class WeatherRequest
    {
        public Location Location { get; set; }
        public bool Details { get; set; } = false;
        public int DaysInterval { get; set; } = 1;
    }
}
