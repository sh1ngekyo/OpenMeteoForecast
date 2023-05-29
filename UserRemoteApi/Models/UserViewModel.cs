using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRemoteApi.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeSpan? Alarm { get; set; }
        public TimeSpan? WarningsStartTime { get; set; }
        public TimeSpan? WarningsEndTime { get; set; }
    }
}
