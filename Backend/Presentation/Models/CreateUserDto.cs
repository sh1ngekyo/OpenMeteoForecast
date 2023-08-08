using AutoMapper;

using System.ComponentModel.DataAnnotations;

using WeatherForecast.Backend.Core.Application.Common.Mappings;
using WeatherForecast.Backend.Core.Application.Users.Commands.CreateUser;

namespace WeatherForecast.Backend.Presentation.UserApi.Models
{
    public class CreateUserDto : IMapWith<CreateUserCommand>
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public double? Latitude { get; set; }
        [Required]
        public double? Longitude { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserDto, CreateUserCommand>()
                .ForMember(userCommand => userCommand.Id,
                    opt => opt.MapFrom(userDto => userDto.Id))
                .ForMember(userCommand => userCommand.Longitude,
                    opt => opt.MapFrom(userDto => userDto.Longitude))
                .ForMember(userCommand => userCommand.Latitude,
                    opt => opt.MapFrom(userDto => userDto.Latitude));
        }
    }
}
