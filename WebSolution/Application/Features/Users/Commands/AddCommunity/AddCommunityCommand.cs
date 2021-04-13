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

namespace Application.Features.Users.Commands.AddCommunity
{
    public class AddCommunityCommand : IRequest
    {
        public int CommunityId { get; set; }
        public string UserEmail { get; set; }
    }

    public class AddCommunityHandler : IRequestHandler<AddCommunityCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddCommunityHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(AddCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Communities.FindAsync(request.CommunityId);
            if (community == null)
                throw new NotFoundException(nameof(Community), request.CommunityId);

            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(request.UserEmail));
            if(user == null)
                throw new NotFoundException(nameof(User), request.UserEmail);
            //todo: verify user is not already in a community


            user.Community = community;

            _context.Users.Update(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

