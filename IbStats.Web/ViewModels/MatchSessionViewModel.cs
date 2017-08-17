using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class MatchSessionViewModel
    {
        public MatchSession MatchSession { get; set; }
        public List<Player> AllPlayers { get; set; }
        public List<Season> AllSeasons { get; set; }
        public int[] SelectedPlayerIdsTeam1 { get; set; }
        public int[] SelectedPlayerIdsTeam2 { get; set; }
    }
}