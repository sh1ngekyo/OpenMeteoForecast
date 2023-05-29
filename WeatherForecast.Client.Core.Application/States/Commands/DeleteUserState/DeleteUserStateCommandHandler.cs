using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;

namespace WeatherForecast.Client.Core.Application.States.Commands.DeleteUserState
{
    public class DeleteUserStateCommandHandler : IRequestHandler<DeleteUserStateCommand, Unit>
    {
        private readonly IUserStateDbContext _context;

        public DeleteUserStateCommandHandler(IUserStateDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserStateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UsersStates.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity is null)
            {
                return Unit.Value;
            }

            _context.UsersStates.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
