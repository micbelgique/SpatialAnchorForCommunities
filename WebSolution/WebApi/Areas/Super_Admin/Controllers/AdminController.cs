using Application.Features.Users.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace WebApi.Areas.Super_Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area("Super_Admin")]
    [Authorize(AuthenticationSchemes = "Identity.Application", Roles = "Super_Admin")]
    public class AdminController : Controller
    {
        private IMediator _mediator;
        private readonly UserManager<User> _userManager;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            return View();
        }


        




    }
}
