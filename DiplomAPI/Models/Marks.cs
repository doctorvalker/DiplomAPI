using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomAPI.Models
{
    public class Marks
    {
        public int markId { get; set; }
        public int eventId { get; set; }
        public int userId { get; set; }
        public float mark { get; set; }
    }
}
