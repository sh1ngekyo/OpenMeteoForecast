using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherForecast.Client.Core.Domain.Models;

namespace UserRemoteApi.Interfaces
{
    public interface IUserManageService
    {
        Task<Response<TResponse>> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
