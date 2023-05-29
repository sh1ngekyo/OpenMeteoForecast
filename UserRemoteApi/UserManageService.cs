using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRemoteApi.Interfaces;

using WeatherForecast.Client.Core.Domain.Models;
using WeatherForecast.Client.Core.Domain.Models.Enums;

namespace UserRemoteApi
{
    public class UserManageService : IUserManageService
    {
        private readonly IMediator _mediator;

        public UserManageService(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Response<TResponse> SendResultWithError<TResponse>(Exception exception)
        {
            var resultWithError = new Response<TResponse>()
            {
                Error = ErrorMessageType.Unknown
            };
            if (exception is HttpRequestException { } ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        resultWithError.Error = ErrorMessageType.NotFound;
                        return resultWithError;
                    case System.Net.HttpStatusCode.Conflict:
                        resultWithError.Error = ErrorMessageType.UserAlreadyExist;
                        return resultWithError;
                    default:
                        resultWithError.Error = ErrorMessageType.InternalServerError;
                        return resultWithError;
                }
            }
            return resultWithError;
        }

        public async Task<Response<TResponse>> HandleAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            try
            {
                return new Response<TResponse>()
                {
                    Result = await _mediator.Send(request, cancellationToken)
                };
            }
            catch (Exception ex)
            {
                return SendResultWithError<TResponse>(ex);
            }
        }
    }
}
