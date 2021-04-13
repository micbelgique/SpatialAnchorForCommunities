using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Dtos
{
    public class FullAnchorsDTO : AnchorDTO
    {
        public List<InteractionDTO> Interactions { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Anchor, FullAnchorsDTO>()
                .ForMember(d => d.Interactions, opt => opt.MapFrom(s => s.Interactions))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id));
        }
    }
}