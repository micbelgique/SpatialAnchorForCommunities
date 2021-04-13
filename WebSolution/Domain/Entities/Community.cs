using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Community : BaseEntity
    {
        #region PROPERTIES
        public String Name { get; set; }
        public String PictureUrl { get; set; }
        public Point EpiCenter { get; set; }
        public int EpiCenterRadius { get; set; }
        public String InfoUrl { get; set; }
        public String Address { get; set; }
        
        public ICollection<User> Users { get; set; }
        #endregion

        #region CONSTRUCTORS
        public Community()
        {
            EpiCenter = new Point(0, 0);
        }
        #endregion
    }
}
