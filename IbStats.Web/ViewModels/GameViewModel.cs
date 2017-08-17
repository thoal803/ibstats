using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class GameViewModel
    {
        public IEnumerable<MatchSession> MatchSessions { get; set; }
        public Season Season { get; set; }
        public List<Season> AllSeasons { get; set; }
    }
}