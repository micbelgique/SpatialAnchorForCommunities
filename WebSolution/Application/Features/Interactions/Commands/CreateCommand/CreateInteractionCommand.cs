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

namespace Application.Features.Interactions.Commands.CreateCommand
{
    public class CreateInteractionCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public string AnchorIdentifier { get; set; }
        public string Message { get; set; }
    }

    public class CreateInteractionHandler : IRequestHandler<CreateInteractionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateInteractionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateInteractionCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var anchor = _context.Anchors.FirstOrDefault(x => x.Identifier.Equals(request.AnchorIdentifier));
            if (anchor == null)
                throw new NotFoundException(nameof(Anchor), request.AnchorIdentifier);

            var entity = new Interaction()
            {
                Anchor = anchor,
                CreationDate = DateTime.Now,
                User = user,
                Message = request.Message
            };

            await _context.Interactions.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
