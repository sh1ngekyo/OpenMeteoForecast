using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Client.Core.Domain.Models.Notifications;

namespace WeatherForecast.Client.Core.Application.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> NotifyAll(CancellationToken cancellationToken = default);
    }
}
