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
    public class GetInteractionsByUserQuery : IRequest<Response<InteractionDTO>>
    {
        public string UserId { get; set; }
    }

    public class
        GetInteractionsByUserHandler : IRequestHandler<GetInteractionsByUserQuery, Response<InteractionDTO>>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetInteractionsByUserHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<InteractionDTO>> Handle(GetInteractionsByUserQuery request, CancellationToken cancellationToken)
        {
            return new Response<InteractionDTO>()
            {
                Data = _context.Interactions.Where(x => x.User.Id == request.UserId)
                    .ProjectTo<InteractionDTO>(_mapper.ConfigurationProvider).ToList()
            };
        }
    }
}