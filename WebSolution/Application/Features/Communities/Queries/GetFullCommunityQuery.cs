using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Communities.Queries
{
    public class GetFullCommunityQuery : IRequest<FullCommunityDTO>
    {
        public int Id { get; set; }
    }

    public class GetFullCommunityQueryHandler : IRequestHandler<GetFullCommunityQuery, FullCommunityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetFullCommunityQueryHandler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<FullCommunityDTO> Handle(GetFullCommunityQuery request, CancellationToken cancellationToken)
        {
            var result = _context.Communities
                .Where(community => community.Id == request.Id)
                .OrderBy(community => community.Name)
                .ProjectTo<FullCommunityDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            foreach (var userDto in result.Users)
            {
                var user = await _userManager.FindByIdAsync(userDto.Id);
                userDto.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            }

            return result;
        }
    }
}