using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IbStats.Models
{
    public class Season
    {
        public int SeasonID { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }
        public String Name { get; set; }
    }
}