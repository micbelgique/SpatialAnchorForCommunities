using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Application.Features.Anchors.Commands.CreateCommand
{
    public class CreateAnchorCommand : IRequest<int>
    {
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string UserId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public int SRID { get; set; }
    }

    public class CreateAnchorCommandHandler : IRequestHandler<CreateAnchorCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateAnchorCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateAnchorCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Find(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var now = DateTime.Now;
            //todo: get adresse from google API with lon/lat
            
            var entity = new Anchor()
            {
                Identifier = request.Identifier,
                Model = request.Model,
                User = user,
                Address = "placeholder",
                CreationDate = now,
                LastUpdateDate = now,
                Location = new Point(request.Latitude, request.Longitude){SRID = request.SRID}
            };

            _context.Anchors.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
