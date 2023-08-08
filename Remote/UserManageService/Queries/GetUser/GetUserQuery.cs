using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserRemoteApi.Models;

namespace UserRemoteApi.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserViewModel?>
    {
        public long Id { get; set; }
    }
}
