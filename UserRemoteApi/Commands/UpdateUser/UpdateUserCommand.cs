using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Models;

namespace UserRemoteApi.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public long Id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Time? Alarm { get; set; }
        public Time? WarningsStartTime { get; set; }
        public Time? WarningsEndTime { get; set; }
    }
}
