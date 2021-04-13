using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public int Id { get; set; }
        public String NickName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String SocialMedia { get; set; }
        public String Enterprise { get; set; }
        public String Mission { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Users.Find(request.Id);

            if (entity == null)
                throw new NotFoundException(nameof(User), request.Id);

            entity.NickName = request.NickName != null ? request.NickName : entity.NickName;
            entity.Email = request.Email != null ? request.Email : entity.Email;
            entity.PhoneNumber = request.PhoneNumber != null ? request.PhoneNumber : entity.PhoneNumber;
            entity.SocialMedia = request.SocialMedia != null ? request.SocialMedia : entity.SocialMedia;
            entity.Enterprise = request.Enterprise != null ? request.Enterprise : entity.Enterprise;
            entity.Mission = request.Mission != null ? request.Mission : entity.Mission;

            _context.Users.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
