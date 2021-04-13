using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.DemoteUser
{
    public class DemoteUserCommand : IRequest
    {
        public string UserId { get; set; }
    }

    public class DemoteUserHandler : IRequestHandler<DemoteUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public DemoteUserHandler(IApplicationDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(DemoteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            _userManager.RemoveFromRoleAsync(user, "Community_Admin").Wait();
            _userManager.AddToRoleAsync(user, "User").Wait();

            return Unit.Value;
        }
    }
}