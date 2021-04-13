using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using Application.Dtos.GeoLocation;

namespace Application.Dtos
{
    public class AnchorDTO : IMapFrom<Anchor>
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Address { get; set; }

        public string PictureUrl { get; set; }

        //public UserDTO User { get; set; }
        public string UserId { get; set; }

        public CoordinateDTO Location { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Anchor, AnchorDTO>()
                .ForMember(d => d.Location, opt => opt.MapFrom(s => new CoordinateDTO(s.Location.X, s.Location.Y){SRID = s.Location.SRID}))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id));
        }
    }
}
