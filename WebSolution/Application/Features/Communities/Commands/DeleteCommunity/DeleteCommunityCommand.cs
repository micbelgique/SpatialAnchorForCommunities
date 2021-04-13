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

namespace Application.Features.Communities.Commands.DeleteCommunity
{
    public class DeleteCommunityCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteCommunityHandler : IRequestHandler<DeleteCommunityCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCommunityHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Communities.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Community), request.Id);
            }

            _context.Communities.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
