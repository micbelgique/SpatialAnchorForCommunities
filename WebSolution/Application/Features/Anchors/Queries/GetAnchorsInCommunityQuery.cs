using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Anchors.Queries
{
    public class GetAnchorsInCommunityQuery : IRequest<Response<AnchorDTO>>
    {
        public int CommunityId { get; set; }
    }

    public class GetAnchorsInCommunityHandler : IRequestHandler<GetAnchorsInCommunityQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAnchorsInCommunityHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<AnchorDTO>> Handle(GetAnchorsInCommunityQuery request, CancellationToken cancellationToken)
        {
            var community = await _context.Communities.FindAsync(request.CommunityId);
            if (community == null)
                throw new NotFoundException(nameof(community), request.CommunityId);

            var temp = _context.Anchors.Include(a => a.User);
            var anchors = new List<AnchorDTO>();
            foreach (var tempAnchor in temp.ToList())
            {
                var distance = tempAnchor.Location.Distance(community.EpiCenter);
                if(distance <= community.EpiCenterRadius)
                {
                    anchors.Add(_mapper.Map<Anchor,AnchorDTO>(tempAnchor));
                }
            }
            
            /*
            var anchors = 
                _context.Anchors
                .Where(a => (a.Location.Distance(community.EpiCenter) <= community.EpiCenterRadius))
                .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider)
                .ToList();
            */

            return new Response<AnchorDTO>()
            {
                Data = anchors
            };
        }
    }
}