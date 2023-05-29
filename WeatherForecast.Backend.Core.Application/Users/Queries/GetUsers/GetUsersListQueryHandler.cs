using AutoMapper;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Backend.Core.Application.Interfaces;

namespace WeatherForecast.Backend.Core.Application.Users.Queries.GetUsers
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserListViewModel>
    {
        private readonly IUserDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IUserDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserListViewModel> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var usersQuery = await _dbContext.Users
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new UserListViewModel { Users = usersQuery };
        }
    }
}
