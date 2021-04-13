using Application.Features.Users.Actions.LoginAction;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginAction action)
        {
            try
            {
                return Ok(await Mediator.Send(action));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
