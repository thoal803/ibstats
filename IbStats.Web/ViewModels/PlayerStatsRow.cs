using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class PlayerStatsRow
    {
        public Player Player { get; set; }
        public int TotalMatchSessionCount { get; set; }
        public int MatchSessionCount { get; set; }
        public int MatchCount { get; set; }
        public int MatchesWonCount { get; set; }
        public int MatchesLostCount { get { return MatchCount - MatchesWonCount; } }
        public int MatchDiff { get { return MatchesWonCount - MatchesLostCount;  } }
        public int GoalsScored { get; set; }
        public int GoalsAgainst { get; set; }
        public float Rating { get { return (TotalMatchSessionCount > 0 ? (MatchDiff + (0.5f * MatchSessionCount) + (0.1f * GoalsScored)) * ((float)MatchSessionCount / ((float)TotalMatchSessionCount)) : 0); } }
        public int TeamGoals { get; set; }
        public List<float> RatingTrend { get; set; }
    }
}