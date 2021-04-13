using Application.Common.Queries;
using Application.Dtos;

namespace WebApi.Areas.Super_Admin.Models.Users
{
    public class ManageUsersModel
    {
        public Response<UserDTO> Users { get; set; }
        public Response<CommunityDTO> Communities { get; set; }
    }
}