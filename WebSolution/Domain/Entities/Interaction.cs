using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Interaction : BaseEntity
    {
        public Anchor Anchor { get; set; }
        public DateTime CreationDate { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
    }
}
