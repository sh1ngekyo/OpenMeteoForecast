using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Backend.Core.Application.Common.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string name, object key) : base($"Entity: '{name}' with Id: ({key}) already exist")
        { }
    }
}
