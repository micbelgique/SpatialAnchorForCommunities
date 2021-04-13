using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public String NickName { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public override String Email { get; set; }
        public override String PhoneNumber { get; set; }
        public String SocialMedia { get; set; }
        public String Enterprise { get; set; }
        public String Mission { get; set; }
        public Community Community { get; set; }

        public List<Anchor> Anchors { get; set; }
        public List<Interaction> Interactions { get; set; }
    }
}
