using HttpService;

using MediatR;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRemoteApi.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public RegisterUserCommandHandler(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _httpService.PostAsJsonAsync<RegisterUserCommand>($"{_configuration.GetRequiredSection("ApiPaths")["Backend"]}", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return Unit.Value;
        }
    }
}
