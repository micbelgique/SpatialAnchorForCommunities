using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Anchors.Commands.UpdateCommand
{
    public class UpdateAnchorPictureCommand : IRequest<string>
    {
        public string AnchorId { get; set; }
        public string PictureUrl { get; set; }
    }

    public class UpdateAnchorPictureHandler : IRequestHandler<UpdateAnchorPictureCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAnchorPictureHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(UpdateAnchorPictureCommand request, CancellationToken cancellationToken)
        {
            var anchor = _context.Anchors.FirstOrDefault(a => a.Identifier == request.AnchorId);
            if (anchor == null)
                throw new NotFoundException(nameof(Anchor), request.AnchorId);

            anchor.PictureUrl = request.PictureUrl;

            _context.Anchors.Update(anchor);

            await _context.SaveChangesAsync(cancellationToken);

            return request.PictureUrl;
        }
    }
}
