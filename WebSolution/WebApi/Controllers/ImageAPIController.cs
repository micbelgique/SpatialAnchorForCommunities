using Application.Common.Services;
using Application.Features.Anchors.Commands.UpdateCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ImageAPIController : ApiControllerBase
    {
        private readonly ImageService _imageService;

        public ImageAPIController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        [Route("uploadTest")]
        public async Task<IActionResult> UploadTest(string anchorId)
        {
            var stream = Request.Body;
            string uri = await _imageService.UploadImageAsync(stream, Guid.NewGuid());
            
            return Ok(await Mediator.Send(new UpdateAnchorPictureCommand() { AnchorId = anchorId, PictureUrl = uri }));
        }
    }
}
