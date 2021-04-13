using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities;
using MediatR;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands.CreateCommunity
{
    public class CreateCommunityCommand : IRequest<int>
    {
        public CommunityDTO CommunityDTO {get; private set;}

        public CreateCommunityCommand(CommunityDTO community)
        {
            if (community == null)
                throw new Exception("Exception");

            CommunityDTO = community;
        }
    }

    public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCommunityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
        {
            var communityDto = request.CommunityDTO;

            var entity = new Community()
            {
                Name = communityDto.Name,
                PictureUrl = communityDto.PictureUrl,
                Address = communityDto.Address,
                EpiCenter = new Point(communityDto.EpiCenter.Longitude, communityDto.EpiCenter.Latitude)
                { SRID = communityDto.EpiCenter.SRID },
                EpiCenterRadius = communityDto.EpiCenter.Radius,
                InfoUrl = communityDto.InfoUrl
            };

            _context.Communities.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
