using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IbStats.Models;

namespace IbStats.ViewModels
{
    public class EditMatchViewModel
    {
        public int MatchSessionID { get; set; }
        public int CurrentMatchID { get; set; }
        public Match Match { get; set; }
    }
}