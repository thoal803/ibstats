using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IbStats.Models
{
    public class MatchSession
    {
        public int MatchSessionID { get; set; }
        public Season Season { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime MatchDate { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public ICollection<Match> Matches { get; set; }
        [Required]
        public Player FirstWasher { get; set; }
        [Required]
        public Player SecondWasher { get; set; }
    }
}