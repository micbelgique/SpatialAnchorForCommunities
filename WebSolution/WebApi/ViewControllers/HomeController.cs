using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Communities.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Models;

namespace WebApi.ViewControllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var model = new IndexModel();
            model.Communities = await Mediator.Send(new GetAllCommunitiesQuery());
            
            return View(model);
        }

        [HttpGet("Authenticate")]
        public IActionResult Authenticate()
        {
            return RedirectToAction("Index");
        }

        [HttpGet("/testJWT")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Test()
        {
            return Ok("test");
        }

        [HttpGet("/testCookie")]
        [Authorize(AuthenticationSchemes = "Identity.Application")] 
        public async Task<IActionResult> Testcookie()
        {
            return Ok("test");
        }
    }
}
