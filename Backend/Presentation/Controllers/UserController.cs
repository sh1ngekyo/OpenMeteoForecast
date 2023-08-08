using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Backend.Core.Application.Users.Commands.CreateUser;
using WeatherForecast.Backend.Core.Application.Users.Commands.DeleteUser;
using WeatherForecast.Backend.Core.Application.Users.Commands.UpdateUser;
using WeatherForecast.Backend.Core.Application.Users.Queries.GetUser;
using WeatherForecast.Backend.Core.Application.Users.Queries.GetUsers;
using WeatherForecast.Backend.Core.Application.Users.Queries;
using WeatherForecast.Backend.Presentation.UserApi.Models;

namespace WeatherForecast.Backend.Presentation.UserApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<UserListViewModel>> GetAll()
        {
            var query = new GetUserListQuery();
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserViewModel>> Get(long id)
        {
            var query = new GetUserQuery
            {
                Id = id
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<long>> Create([FromBody] CreateUserDto user)
        {
            var command = new CreateUserCommand
            {
                Id = user.Id,
                Longitude = user.Longitude,
                Latitude = user.Latitude,
            };
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto user)
        {
            var command = new UpdateUserCommand
            {
                Id = user.Id,
                Longitude = user.Longitude,
                Latitude = user.Latitude,
                Alarm = user.Alarm,
                WarningsStartTime = user.WarningsStartTime,
                WarningsEndTime = user.WarningsEndTime,
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteUserCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
