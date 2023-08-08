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

namespace WeatherForecast.Backend.Core.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserDbContext _context;

        public UpdateUserCommandHandler(IUserDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            entity.Longitude = request.Longitude.HasValue ? request.Longitude.Value : entity.Longitude;
            entity.Latitude = request.Latitude.HasValue ? request.Latitude.Value : entity.Latitude;
            entity.AlarmTime = request.Alarm.HasValue ? request.Alarm.Value.Value : entity.AlarmTime;
            entity.WarningsStartTime = request.WarningsStartTime.HasValue ? request.WarningsStartTime.Value.Value : entity.WarningsStartTime;
            entity.WarningsEndTime = request.WarningsEndTime.HasValue ? request.WarningsEndTime.Value.Value : entity.WarningsEndTime;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
