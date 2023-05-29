using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Backend.Core.Application.Common.Exceptions;
using WeatherForecast.Backend.Core.Application.Interfaces;

using WeatherForecast.Backend.Core.Domain;

namespace WeatherForecast.Backend.Core.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
    {
        private readonly IUserDbContext _context;

        public CreateUserCommandHandler(IUserDbContext context)
        {
            _context = context;
        }

        public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync(user => user.Id == request.Id);
            if (userExist != null)
            {
                throw new AlreadyExistException(nameof(User), request.Id!.Value);
            }

            var user = new User
            {
                Id = request.Id!.Value,
                Latitude = request.Latitude!.Value,
                Longitude = request.Longitude!.Value,
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
