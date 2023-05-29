using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;

namespace WeatherForecast.Client.Core.Application.Interfaces
{
    public interface IUserStateDbContext
    {
        DbSet<UserState> UsersStates { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
