using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;

namespace WeatherForecast.Client.Core.Application.States.Commands.CreateUserState
{
    public class CreateUserStateHandler : IRequestHandler<CreateUserStateCommand, Domain.Models.UserState>
    {
        private readonly IUserStateDbContext _context;

        public CreateUserStateHandler(IUserStateDbContext userStateDbContext)
        {
            _context = userStateDbContext;
        }

        public async Task<Domain.Models.UserState> Handle(CreateUserStateCommand request, CancellationToken cancellationToken)
        {
            var userState = await _context.UsersStates.FirstOrDefaultAsync(state => state.UserId == request.Id);
            if (userState != null)
            {
                return userState;
            }

            userState = new Domain.Models.UserState
            {
                UserId = request.Id,
                StateType = Domain.Models.Enums.StateType.WaitingForNewMessages
            };

            await _context.UsersStates.AddAsync(userState, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return userState;
        }
    }
}
