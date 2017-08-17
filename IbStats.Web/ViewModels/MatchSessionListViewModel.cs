using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class MatchSessionListViewModel
    {
        public Season Season { get; set; }
        public List<Season> AllSeasons { get; set; }
        public List<MatchSession> MatchSessions { get; set; }
    }
}