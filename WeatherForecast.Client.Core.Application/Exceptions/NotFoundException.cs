using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, long id) : base($"Entity '{name}' with Id: '{id}' not found and has no state.") { }
    }
}
