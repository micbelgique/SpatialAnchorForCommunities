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
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<Response<UserDTO>>
    {

    }

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Response<UserDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetAllUsersHandler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Response<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = new Response<UserDTO>()
            {
                Data = _context.Users
                    .Include(user => user.Community)
                    .OrderBy(user => user.NickName)
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .ToList()
            };

            foreach (var userDto in users.Data)
            {
                var user = await _userManager.FindByIdAsync(userDto.Id);
                userDto.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            }

            return users;
        }
    }
}
