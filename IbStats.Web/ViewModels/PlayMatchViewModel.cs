using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class PlayMatchViewModel
    {
        public MatchSession MatchSession { get; set; }
        public Match CurrentMatch { get; set; }
        public ICollection<Match> MatchList { get; set; }
    }
}