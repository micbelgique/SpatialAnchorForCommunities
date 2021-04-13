using Application.Common.Queries;
using Application.Dtos;

namespace WebApplication.Models
{
    public class IndexModel
    {
        public Response<AnchorDTO> Anchors { get; set; }
        public Response<CommunityDTO> Communities { get; set; }
    }
}