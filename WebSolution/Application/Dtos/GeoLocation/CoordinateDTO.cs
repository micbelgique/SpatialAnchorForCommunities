using Application.Common.Mappings;
using AutoMapper;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.GeoLocation
{
    public class CoordinateDTO : IMapFrom<Point>
    {
        #region PROPERTIES
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int SRID { get; set; }
        #endregion

        #region CONSTRUCTORS
        public CoordinateDTO()
        {
        }

        public CoordinateDTO(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
        #endregion

        #region MAPPING 
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Point, CoordinateDTO>()
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.X))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Y))
                .ForMember(d => d.SRID, opt => opt.MapFrom(s => s.SRID));
        }
        #endregion
    }
}
