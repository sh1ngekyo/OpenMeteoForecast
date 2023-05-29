using AutoMapper;

using System.ComponentModel.DataAnnotations;

using WeatherForecast.Backend.Core.Application.Common.Mappings;
using WeatherForecast.Backend.Core.Application.Users.Commands.UpdateUser;
using WeatherForecast.Backend.Core.Domain;

namespace WeatherForecast.Backend.Presentation.UserApi.Models
{
    public class UpdateUserDto : IMapWith<UpdateUserCommand>
    {
        [Required]
        public long? Id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Time? Alarm { get; set; }
        public Time? WarningsStartTime { get; set; }
        public Time? WarningsEndTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserDto, UpdateUserCommand>()
                .ForMember(userCommand => userCommand.Id,
                    opt => opt.MapFrom(userDto => userDto.Id))
                .ForMember(userCommand => userCommand.Longitude,
                    opt => opt.MapFrom(userDto => userDto.Longitude))
                .ForMember(userCommand => userCommand.Latitude,
                    opt => opt.MapFrom(userDto => userDto.Latitude))
                .ForMember(userCommand => userCommand.Alarm,
                    opt => opt.MapFrom(userDto => userDto.Alarm))
                .ForMember(userCommand => userCommand.WarningsStartTime,
                    opt => opt.MapFrom(userDto => userDto.WarningsStartTime))
                .ForMember(userCommand => userCommand.WarningsEndTime,
                    opt => opt.MapFrom(userDto => userDto.WarningsEndTime));
        }
    }
}
