using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserDTO : IMapFrom<User>
    {
        public String Id { get; set; }
        public String NickName { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String SocialMedia { get; set; }
        public String Enterprise { get; set; }
        public String Mission { get; set; }
        public int CommunityId { get; set; }
        
        public string Role { get; set; }

        #region MAPPING
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDTO>()
                .ForMember(d => d.NickName, opt => opt.MapFrom(s => s.UserName ))
                .ForMember(d => d.CommunityId, opt => opt.MapFrom(s => (s.Community == null ? -1 : s.Community.Id) ));
        }
        #endregion
    }
}
