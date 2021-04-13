using System.ComponentModel.DataAnnotations;
using Application.Dtos;

namespace WebApi.Areas.Super_Admin.Models.Users
{
    public class EditUserModel
    {
        public UserDTO User { get; set; }
        
        [Display(Name = "Community admin")]
        public bool IsCommunityAdmin { get; set; }

        public EditUserModel()
        {
            IsCommunityAdmin = User?.Role == "Community_Admin";
        }

        public EditUserModel(UserDTO user)
        {
            User = user;
            IsCommunityAdmin = User?.Role == "Community_Admin";
        }
        
    }
}