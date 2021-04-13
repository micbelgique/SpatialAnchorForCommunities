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

namespace Application.Features.Anchors.Commands.DeleteCommand
{
    public class DeleteAnchorCommand : IRequest
    {
        public string Identifier { get; set; }
    }

    public class DeleteAnchorHandler : IRequestHandler<DeleteAnchorCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAnchorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAnchorCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Anchors.Where(anchor => anchor.Identifier == request.Identifier).FirstOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(User), request.Identifier);
            }

            _context.Anchors.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
