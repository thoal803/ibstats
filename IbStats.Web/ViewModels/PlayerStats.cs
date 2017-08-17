using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class PlayerStats
    {
        public IEnumerable<PlayerStatsRow> PlayerStatsRows { get; set; }
        public int TotalMatchSessionCount { get; set; }
        public Season Season { get; set; }
        public List<Season> AllSeasons { get; set; }
    }
}