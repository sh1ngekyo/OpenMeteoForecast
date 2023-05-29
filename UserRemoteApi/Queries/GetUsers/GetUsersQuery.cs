using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Models;

namespace UserRemoteApi.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<UsersList>
    {
    }
}
