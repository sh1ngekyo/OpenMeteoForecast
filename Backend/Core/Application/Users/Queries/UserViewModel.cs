using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Backend.Core.Application.Common.Mappings;

using WeatherForecast.Backend.Core.Domain;

namespace WeatherForecast.Backend.Core.Application.Users.Queries
{

    public class UserViewModel : IMapWith<User>
    {
        public long Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeSpan? Alarm { get; set; }
        public TimeSpan? WarningsStartTime { get; set; }
        public TimeSpan? WarningsEndTime { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.Id,
                opt => opt.MapFrom(user => user.Id))
                .ForMember(vm => vm.Latitude,
                opt => opt.MapFrom(user => user.Latitude))
                .ForMember(vm => vm.Longitude,
                opt => opt.MapFrom(user => user.Longitude))
                .ForMember(vm => vm.Alarm,
                opt => opt.MapFrom(user => user.AlarmTime))
                .ForMember(vm => vm.WarningsStartTime,
                opt => opt.MapFrom(user => user.WarningsStartTime))
                .ForMember(vm => vm.WarningsEndTime,
                opt => opt.MapFrom(user => user.WarningsEndTime));
        }
    }
}
