using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IbStats.Models
{
    public class Match
    {
        public int MatchID { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}