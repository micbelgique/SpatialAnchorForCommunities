using Application.Common.Interfaces;
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
    public class GetCommunityQuery : IRequest<CommunityDTO>
    {
        public int Id { get; set; }
    }

    public class GetCommunityQueryHandler : IRequestHandler<GetCommunityQuery, CommunityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommunityQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommunityDTO> Handle(GetCommunityQuery request, CancellationToken cancellationToken)
        {
            return _context.Communities
                .OrderBy(community => community.Name)
                .Where(c => c.Id == request.Id)
                .ProjectTo<CommunityDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
        }
    }
}
