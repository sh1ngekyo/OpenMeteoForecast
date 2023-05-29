using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Persistence.EntityTypeConfiguration;

namespace WeatherForecast.Client.Persistence
{
    public class UserStateDbContext : DbContext, IUserStateDbContext
    {
        public UserStateDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserState> UsersStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserStateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
