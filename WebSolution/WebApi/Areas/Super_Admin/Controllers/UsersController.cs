using System;
using System.Threading.Tasks;
using Application.Features.Communities.Queries;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.DemoteUser;
using Application.Features.Users.Commands.PromoteUser;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Areas.Super_Admin.Models.Users;

namespace WebApi.Areas.Super_Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area("Super_Admin")]
    [Authorize(AuthenticationSchemes = "Identity.Application", Roles = "Super_Admin,Community_Admin")]
    public class UsersController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
                
        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var model = new ManageUsersModel();
            model.Users = await Mediator.Send(new GetAllUsersQuery());
            model.Communities = await Mediator.Send(new GetAllCommunitiesQuery());
            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await Mediator.Send(new GetUserByIdQuery(){UserId = id});
            var model = new EditUserModel(user);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var command = new DeleteUserCommand() {UserId = id};
            try
            {
                await Mediator.Send(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpGet("Super_Admin/Users/PromoteUser/{userId}")]
        public async Task<IActionResult> PromoteUser(string userId)
        {
            var command = new PromoteUserCommand() {UserId = userId};
            _ = await Mediator.Send(command);

            return RedirectToAction("EditUser", new {id = userId});
        }
        
        [HttpGet("Super_Admin/Users/DemoteUser/{userId}")]
        public async Task<IActionResult> DemoteUser(string userId)
        {
            var command = new DemoteUserCommand() {UserId = userId};
            _ = await Mediator.Send(command);

            return RedirectToAction("EditUser", new {id = userId});
        }
    }
}