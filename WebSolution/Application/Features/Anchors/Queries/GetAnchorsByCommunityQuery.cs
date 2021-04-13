using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Anchors.Queries
{
    public class GetAnchorsByCommunityQuery : IRequest<Response<AnchorDTO>>
    {
        public int CommunityId { get; set; }
    }

    public class GetAnchorsByCommunityHandler : IRequestHandler<GetAnchorsByCommunityQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAnchorsByCommunityHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Response<AnchorDTO>> Handle(GetAnchorsByCommunityQuery request, CancellationToken cancellationToken)
        {
            //todo verify if community id is valid
            
            return new Response<AnchorDTO>()
            {
                Data = _context.Anchors
                    .Where(anchor => anchor.User.Community.Id == request.CommunityId)
                    .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }
    }
}