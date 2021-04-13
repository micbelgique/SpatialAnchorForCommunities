using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Anchors.Queries
{
    public class GetAllAnchorsQuery : IRequest<Response<AnchorDTO>>
    {
    }

    public class GetAllAnchorsQueryHandler : IRequestHandler<GetAllAnchorsQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllAnchorsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<AnchorDTO>> Handle(GetAllAnchorsQuery request, CancellationToken cancellationToken)
        {
            return new Response<AnchorDTO>()
            {
                Data = _context.Anchors
                    .OrderBy(anchor => anchor.Id)
                    .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }
    }
}
