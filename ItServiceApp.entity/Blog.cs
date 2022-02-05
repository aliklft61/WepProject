using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItServiceApp.entity
{
    public class Blog
    {
        public int BlogID { get; set; }
        public string BlogName { get; set; }
        public string BlogUrl { get; set; }
        public string BlogDescription { get; set; }
        public string BlogImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsHome { get; set; }
    }
}
