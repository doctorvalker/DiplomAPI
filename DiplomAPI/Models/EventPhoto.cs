using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomAPI.Models
{
    public class EventPhoto
    {
        public int photoId { get; set; }
        public int eventId { get; set; }
        public string Photo { get; set; }
    }
}
