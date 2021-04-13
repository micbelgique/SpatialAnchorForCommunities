using System.Collections.Generic;
using Applications.Dtos.GeoLocation;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos
{
    public class FullCommunityDTO : CommunityDTO
    {
        public List<UserDTO> Users { get; set; }
        
        public new void Mapping(Profile profile)
        {
            profile.CreateMap<Community, FullCommunityDTO>()
                .ForMember(d => d.EpiCenter, 
                    opt => opt.MapFrom(s => new EpicenterDTO(s.EpiCenter.X, s.EpiCenter.Y, s.EpiCenterRadius)));
                //.ForMember(d => d.Users, s => s.MapFrom( m => m))
        }
    }
}