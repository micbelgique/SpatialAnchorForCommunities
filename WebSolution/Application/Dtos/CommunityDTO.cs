using Domain.Entities;
using System;
using Application.Common.Mappings;
using AutoMapper;
using Applications.Dtos.GeoLocation;

namespace Application.Dtos
{
    public class CommunityDTO : IMapFrom<Community>
    {
        #region PROPERTIES 

        public int Id { get; set; }
        public String Name { get; set; }
        public String PictureUrl { get; set; }
        public EpicenterDTO EpiCenter { get; set; }
        public String InfoUrl { get; set; }
        public String Address { get; set; }

        #endregion

        #region CONNSTRUCTORS

        public CommunityDTO() { }

        #endregion

        #region MAPPING 

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Community, CommunityDTO>()
                .ForMember(d => d.EpiCenter, opt => opt.MapFrom(s => new EpicenterDTO(s.EpiCenter.X, s.EpiCenter.Y, s.EpiCenterRadius)));
        }

        #endregion
    }
}
