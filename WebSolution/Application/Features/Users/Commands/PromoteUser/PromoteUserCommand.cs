using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.PromoteUser
{
    public class PromoteUserCommand : IRequest
    {
        public string UserId { get; set; }
    }

    public class PromoteUserHandler : IRequestHandler<PromoteUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PromoteUserHandler(IApplicationDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public async Task<Unit> Handle(PromoteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            _userManager.RemoveFromRoleAsync(user, "User").Wait();
            _userManager.AddToRoleAsync(user, "Community_Admin").Wait();
            
            return Unit.Value;
        }
    }
    
    
}