using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Communities.Commands.CreateCommunity;
using Application.Features.Communities.Queries;
using Application.Features.Users.Actions.CheckInCommunity;
using Application.Features.Users.Commands.AddCommunity;
using Application.Features.Users.Queries;
using Applications.Dtos.GeoLocation;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Areas.Super_Admin.Models;

namespace WebApi.Areas.Super_Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area("Super_Admin")]
    [Authorize(AuthenticationSchemes = "Identity.Application", Roles = "Super_Admin, Community_Admin")]
    public class CommunitiesController : Controller
    {
        private IMediator _mediator;
        private readonly UserManager<User> _userManager;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public CommunitiesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Identity.Application", Roles = "Super_Admin")]
        public async Task<IActionResult> Index()
        {
            var communities = await Mediator.Send(new GetAllCommunitiesQuery());
            return View(communities);
        }

        [HttpGet("manage/{id}")]
        public async Task<IActionResult> ManageCommunity(int id)
        {
            if (id == -1)
            {
                var user = await _userManager.GetUserAsync(User);
                var userDto = await Mediator.Send(new GetUserByIdQuery() { UserId = user.Id });
                id = userDto.CommunityId;
            }

            var community = await Mediator.Send(new GetFullCommunityQuery() {Id = id});
            return View(community);
        }
        
        [HttpPost("manage/{community}")]
        public async Task<IActionResult> AddUserToCommunity(int community, [FromForm] AddCommunityCommand command)
        {
            /*
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Community_Admin"))
            {
                var action = new CheckUserInCommunity(user.Id, community);
                var checkInCommunity = await Mediator.Send(action); //checks if
            }
            */

            await Mediator.Send(command);
            return RedirectToAction("ManageCommunity", new {id = community});
        }
        
        [HttpGet("add")]
        [Authorize(AuthenticationSchemes = "Identity.Application", Roles = "Super_Admin")]
        public async Task<IActionResult> AddCommunity()
        {
            return View();
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Identity.Application", Roles = "Super_Admin")]
        public async Task<IActionResult> AddCommunityPost([FromForm] AddCommunityModel model)
        {
            var community = new CommunityDTO()
            {
                Address = model.Address,
                EpiCenter = new EpicenterDTO() {Latitude = model.Latitude, Longitude = model.Longitude, SRID = 4326, Radius = model.Radius},
                InfoUrl = model.InfoUrl,
                Name = model.Name,
                PictureUrl = model.PictureUrl
            };
            var command = new CreateCommunityCommand(community);
            var result = await Mediator.Send(command);
            
            return RedirectToAction("ManageCommunity", new {id = result});
        }
    }
}