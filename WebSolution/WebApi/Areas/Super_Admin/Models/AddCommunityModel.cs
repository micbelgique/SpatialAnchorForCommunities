using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Super_Admin.Models
{
    public class AddCommunityModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Community Name")]
        public string Name { get; set; }
        
        [DataType(DataType.Url)]
        [Display(Name = "Picture Url")]
        public string PictureUrl { get; set; }
        
        [DataType(DataType.Url)]
        [Display(Name = "Community Site")]
        public string InfoUrl { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        
        [Required]
        public float Longitude { get; set; }
        
        [Required]
        public float Latitude { get; set; }
        
        [Required]
        [Range(1, 20, ErrorMessage = "Radius must be between 1 and 20 meters")]
        public int Radius { get; set; }
        
    }
}