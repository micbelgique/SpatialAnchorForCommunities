using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Features.Anchors.Queries
{
    public class GetAnchorsByUserQuery : IRequest<Response<AnchorDTO>>
    {
        public string UserId { get; set;}
    }

    public class GetAnchorsByUserHandler : IRequestHandler<GetAnchorsByUserQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAnchorsByUserHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<AnchorDTO>> Handle(GetAnchorsByUserQuery request,
            CancellationToken cancellationToken)
        {
            //todo verify if community id is valid
            
            return new Response<AnchorDTO>()
            {
                Data = _context.Anchors
                    .Where(anchor => String.Equals(anchor.User.Id, request.UserId))
                    .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }
    }
}