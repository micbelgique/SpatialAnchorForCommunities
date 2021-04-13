using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Actions.CheckInCommunity
{
    public class CheckUserInCommunity : IRequest<bool>
    {
        public string UserId { get; set; }
        public int CommunityId { get; set; }

        public CheckUserInCommunity(string uid, int cid)
        {
            CommunityId = cid;
            UserId = uid;
        }
    }

    public class CheckUserInCommunityHandler : IRequestHandler<CheckUserInCommunity, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckUserInCommunityHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckUserInCommunity request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Include(u => u.Community).FirstOrDefault(u => u.Id.Equals(request.UserId));
            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            return user.Community.Id == request.CommunityId;

        }
    }
}
