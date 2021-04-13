using Application.Features.Interactions.Commands.CreateCommand;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Interactions.Queries;

namespace WebApi.Controllers
{
    public class InteractionsAPIController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostInteraction([FromBody] CreateInteractionCommand command )
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("community")]
        public async Task<IActionResult> GetAnchorsByCommunity([FromQuery] GetInteractionsByCommunityQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        
        [HttpGet("user")]
        public async Task<IActionResult> GetAnchorsByUser([FromQuery] GetInteractionsByUserQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
