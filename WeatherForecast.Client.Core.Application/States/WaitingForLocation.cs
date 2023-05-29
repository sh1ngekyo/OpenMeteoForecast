using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.Core.Application.States
{
    public class WaitingForLocation : IState
    {
        private readonly IUserManageService _userManageService;

        public WaitingForLocation(IUserManageService userManageService)
        {
            _userManageService = userManageService;
        }

        public async Task<Response<OutputMessage>> Handle(IUserContext context, Message message, CancellationToken cancellationToken = default)
        {
            context.UpdateState(StateType.WaitingForNewMessages);
            if (message.Location is null)
            {
                return await context.Update(message, cancellationToken);
            }
            var user = await _userManageService.HandleAsync(
               new GetUserQuery
               {
                   Id = message.Chat.Id
               },
               cancellationToken);
            if (user.HasError)
            {
                if (user.Error == ErrorMessageType.NotFound)
                {
                    return await new RegisterAction(_userManageService).Do(message, cancellationToken);
                }
                return new Response<OutputMessage>()
                {
                    Error = user.Error,
                };
            }
            return await new UpdateAction(_userManageService).Do(message, cancellationToken);
        }
    }
}
