using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomAPI.Models
{
    public class Comments
    {
        public int commentId { get; set; }
        public int eventId { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
    }
}
