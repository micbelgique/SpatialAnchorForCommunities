using Application.Features.Users.Commands.AddCommunity;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    public class UsersAPIController : ApiControllerBase
    {
        private readonly ILogger<UsersAPIController> _logger;

        public UsersAPIController(ILogger<UsersAPIController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByID(string id)
        {
            _logger.Log(LogLevel.Debug, $"user with ID {id} has been requested");
            return Ok(await Mediator.Send(new GetUserByIdQuery() { UserId = id }));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserCommand command)
        {
            return await GetUserByID(await Mediator.Send(command));
        }

        [HttpPost("community")]
        public async Task<IActionResult> AddCommunity([FromBody] AddCommunityCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromBody] DeleteUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
