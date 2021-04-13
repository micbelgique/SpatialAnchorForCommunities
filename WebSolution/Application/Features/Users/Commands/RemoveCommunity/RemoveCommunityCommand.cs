using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.RemoveCommunity
{
    public class RemoveCommunityCommand : IRequest
    {
        public int CommunityId { get; set; }
        public string UserId { get; set; }
    }

    public class RemoveCommunityHandler : IRequestHandler<RemoveCommunityCommand>
    {
        private readonly IApplicationDbContext _context;

        public RemoveCommunityHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(RemoveCommunityCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if(user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            if (user.Community?.Id != request.CommunityId)
                throw new NotFoundException($"user: {user.NickName} is not part of community: {request.CommunityId}");
                
            user.Community = null;

            _context.Users.Update(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}