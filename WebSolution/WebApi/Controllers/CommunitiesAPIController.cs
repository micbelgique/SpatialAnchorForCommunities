using Application.Dtos;
using Application.Features.Communities.Commands.CreateCommunity;
using Application.Features.Communities.Commands.DeleteCommunity;
using Application.Features.Communities.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class CommunitiesAPIController : ApiControllerBase
    {
        private readonly ILogger<CommunitiesAPIController> _logger;

        public CommunitiesAPIController(ILogger<CommunitiesAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCommunitiesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunity(int id, Boolean full = false)
        {
            if (full)
            {
                var result = await Mediator.Send(new GetFullCommunityQuery() { Id = id });
                if (result == null)
                    return NotFound(result);
                else
                    return Ok(result);
            }
            else
            {
                var result = await Mediator.Send(new GetCommunityQuery() { Id = id });
                if (result == null)
                    return NotFound(result);
                else
                    return Ok(result);
            }
            

            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommunityDTO communityDTO)
        {
            return Ok(await Mediator.Send(new CreateCommunityCommand(communityDTO)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCommunityCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}
