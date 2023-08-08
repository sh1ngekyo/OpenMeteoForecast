using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Exceptions;
using WeatherForecast.Client.Core.Application.Interfaces;

namespace WeatherForecast.Client.Core.Application.States.Queries.GetUserState
{
    public class GetUserStateQueryHandler : IRequestHandler<GetUserStateQuery, Domain.Models.UserState>
    {
        private readonly IUserStateDbContext _context;

        public GetUserStateQueryHandler(IUserStateDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Models.UserState> Handle(GetUserStateQuery request, CancellationToken cancellationToken)
        {
            var state = await _context.UsersStates.FirstOrDefaultAsync(state => state.UserId == request.Id, cancellationToken);

            if (state == null)
            {
                throw new NotFoundException(nameof(Domain.Models.UserState), request.Id);
            }

            return state;
        }
    }
}
