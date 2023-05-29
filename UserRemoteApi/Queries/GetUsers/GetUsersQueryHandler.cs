using HttpService;
using MediatR;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Models;

namespace UserRemoteApi.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UsersList>
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public GetUsersQueryHandler(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<UsersList> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var response = await _httpService.GetAsync($"{_configuration.GetRequiredSection("ApiPaths")["Backend"]}", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UsersList>();
        }
    }
}
