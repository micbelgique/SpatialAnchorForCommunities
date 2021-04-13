using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Dtos
{
    public class InteractionDTO : IMapFrom<Interaction>
    {
        public DateTime CreationDate { get; set; }
        //public UserDTO User { get; set; }
        public string UserId { get; set; }
        public string AnchorIdentifier { get; set; }
        public string Message { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Interaction, InteractionDTO>()
                .ForMember(d => d.UserId, 
                    opt => opt.MapFrom(x => x.User.Id))
                .ForMember(d => d.AnchorIdentifier, 
                    opt => opt.MapFrom(x => x.Anchor.Identifier));
        }
    }
}
