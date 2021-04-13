using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries
{
    public class GetAllCommunitiesQuery : IRequest<Response<CommunityDTO>>
    {
    }

    public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, Response<CommunityDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCommunitiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<CommunityDTO>> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
        {
            return new Response<CommunityDTO>() {Data = _context.Communities
                .OrderBy(community => community.Name)
                .ProjectTo<CommunityDTO>(_mapper.ConfigurationProvider)
                .ToList()
            };
        }
    }
}
