using Application.Dtos.GeoLocation;

namespace Applications.Dtos.GeoLocation
{
    public class EpicenterDTO : CoordinateDTO
    {
        public int Radius { get; set; }

        public EpicenterDTO() { }

        public EpicenterDTO(double longitude, double latitude, int radius) : base(longitude, latitude)
        {
            Radius = radius;
        }
    }
}
