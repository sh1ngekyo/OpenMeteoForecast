using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WeatherForecast.Client.Core.Application.Context;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Application.States.Commands.CreateUserState;
using WeatherForecast.Client.Core.Application.States.Commands.DeleteUserState;
using WeatherForecast.Client.Core.Application.States.Commands.UpdateUserState;
using WeatherForecast.Client.Core.Application.States.Queries.GetUserState;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application
{
    public class UserManager : IUserManager
    {
        private readonly IMediator _mediator;
        private readonly IStateStrategy _stateStrategy;

        public UserManager(IMediator mediator, IStateStrategy stateStrategy)
        {
            _mediator = mediator;
            _stateStrategy = stateStrategy;
        }

        public async Task Add(long userId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new CreateUserStateCommand
            {
                Id = userId
            }, cancellationToken);
        }

        public async Task Remove(long userId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteUserStateCommand
            {
                Id = userId
            }, cancellationToken);
        }

        public async Task<Response<OutputMessage>> HandleMessage(long userId, Message message, CancellationToken cancellationToken = default)
        {
            var userState = await _mediator.Send(new GetUserStateQuery
            {
                Id = userId
            }, cancellationToken);
            var ctx = new UserContext(_stateStrategy, userState.StateType);
            var response = await ctx.Update(message, cancellationToken);
            await _mediator.Send(new UpdateUserStateCommand
            {
                Id = userId,
                StateType = ctx.GetStateType(),
            }, cancellationToken);
            return response;
        }
    }
}
