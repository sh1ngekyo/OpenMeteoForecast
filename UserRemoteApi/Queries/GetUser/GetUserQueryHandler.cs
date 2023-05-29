using HttpService;
using MediatR;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Models;

namespace UserRemoteApi.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel?>
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public GetUserQueryHandler(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<UserViewModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var response = await _httpService.GetAsync($"{_configuration.GetRequiredSection("ApiPaths")["Backend"]}/{request.Id}", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserViewModel?>();
        }
    }
}
