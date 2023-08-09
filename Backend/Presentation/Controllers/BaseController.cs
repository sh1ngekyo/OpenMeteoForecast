﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.Backend.Presentation.UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    }
}
