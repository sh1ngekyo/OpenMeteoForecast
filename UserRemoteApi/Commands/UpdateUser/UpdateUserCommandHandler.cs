using HttpService;
using MediatR;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRemoteApi.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public UpdateUserCommandHandler(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _httpService.PutAsJsonAsync<UpdateUserCommand>($"{_configuration.GetRequiredSection("ApiPaths")["Backend"]}", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return Unit.Value;
        }
    }
}
