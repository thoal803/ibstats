using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IbStats.Models
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }
        public int TeamID { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}