using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Features.Interactions.Queries
{
    public class GetInteractionsByCommunityQuery : IRequest<Response<InteractionDTO>>
    {
        public int CommunityId { get; set; }
    }

    public class
        GetInteractionsByCommunityHandler : IRequestHandler<GetInteractionsByCommunityQuery, Response<InteractionDTO>>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetInteractionsByCommunityHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<InteractionDTO>> Handle(GetInteractionsByCommunityQuery request, CancellationToken cancellationToken)
        {
            return new Response<InteractionDTO>()
            {
                Data = _context.Interactions.Where(x => x.User.Community.Id == request.CommunityId)
                    .ProjectTo<InteractionDTO>(_mapper.ConfigurationProvider).ToList()
            };
        }
    }
}