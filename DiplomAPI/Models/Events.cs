using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomAPI.Models
{
    public class Events
    {
        public int eventId { get; set; }
        public string eventName { get; set; }
        public string eventDescription { get; set; }
        public string eventPicture { get; set; }
        public string dateOfStart { get; set; }
        public string dateOfEnd { get; set; }
        public string address { get; set; }
        public int tagId { get; set; }
    }
}
