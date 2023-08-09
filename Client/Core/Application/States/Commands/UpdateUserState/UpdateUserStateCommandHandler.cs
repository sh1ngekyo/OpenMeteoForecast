using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace WeatherForecast.Client.Core.Application.States.Commands.UpdateUserState
{
    public class UpdateUserStateCommandHandler : IRequestHandler<UpdateUserStateCommand, Unit>
    {
        private readonly IUserStateDbContext _context;

        public UpdateUserStateCommandHandler(IUserStateDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserStateCommand request, CancellationToken cancellationToken)
        {
            var state = await _context.UsersStates.FirstOrDefaultAsync(state => state.UserId == request.Id, cancellationToken);

            if (state is null)
            {
                return Unit.Value;
            }

            state.StateType = request.StateType;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
