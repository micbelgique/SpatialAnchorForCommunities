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
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Anchors.Queries
{
    public class GetAnchorByIdentifierQuery : IRequest<FullAnchorsDTO>
    {
        public String Identifier { get; set; }

        public GetAnchorByIdentifierQuery(string identifier)
        {
            this.Identifier = identifier;
        }
    }

    public class GetAnchorByIdentifierHandler : IRequestHandler<GetAnchorByIdentifierQuery, FullAnchorsDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAnchorByIdentifierHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FullAnchorsDTO> Handle(GetAnchorByIdentifierQuery request, CancellationToken cancellationToken)
        {
            return _context.Anchors
                .Include(x => x.Interactions)
                .Where(anchor => anchor.Identifier.Equals(request.Identifier))
                .ProjectTo<FullAnchorsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
        }
    }
}
