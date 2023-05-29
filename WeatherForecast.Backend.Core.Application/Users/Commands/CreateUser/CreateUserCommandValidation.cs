using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Backend.Core.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(createUserCommand => createUserCommand.Id).NotNull();
            RuleFor(createUserCommand => createUserCommand.Latitude).NotNull();
            RuleFor(createUserCommand => createUserCommand.Longitude).NotNull();
        }
    }
}
