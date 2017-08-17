using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IbStats.Models
{
    public class Goal
    {
        public int GoalID { get; set; }
        public Player Scorer { get; set; }
        public DateTime? RegistrationTime { get; set; }
    }
}