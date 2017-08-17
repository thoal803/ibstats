using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IbStats.Models;
using IbStats.ViewModels;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IbStats.Controllers
{
    public class HomeController : IbStatsController
    {
        public async Task<ActionResult> Index(int? seasonID)
        {
            bool marathonTable = false;
            List<PlayerStatsRow> rows = new List<PlayerStatsRow>();
            PlayerStats model = new PlayerStats();
            if (seasonID == null)
            {
                model.Season = await GetCurrentSeasonAsync();
            }
            else if (seasonID == 9999)
            {
                marathonTable = true;
                model.Season = new Season() { SeasonID = 9999, Name = "Marathontabell" };
            }
            else
            {
                model.Season = await db.Seasons.Where(s => s.SeasonID == seasonID).FirstOrDefaultAsync<Season>();
            }

            model.AllSeasons = await GetAllSeasonsAsync();

            model.AllSeasons.Add(new Season() { SeasonID = 9999, Name = "Marathontabell" });
            
            if(marathonTable)
                model.TotalMatchSessionCount = await db.MatchSessions.CountAsync<MatchSession>();
            else
                model.TotalMatchSessionCount = await db.MatchSessions.Where(m => m.Season.SeasonID == model.Season.SeasonID).CountAsync<MatchSession>();


            foreach (Player p in db.Players.ToList<Player>().Where(x => x.Name != "Blå" && x.Name != "Gul"))
            {
                // Calculate played MatchSessions
                int matchSessionCount = 0;
                int matchCount = 0;
                int goalCount = 0;
                int teamGoalCount = 0;
                int wonMatchesCount = 0;
                int goalsAgainst = 0;
                /*
                int lastParticipatingMatchSession = 0;
                int totalMatchSessionCount = db.MatchSessions.Where(x => x.Season.SeasonID == model.Season.SeasonID).Count();
                */
                float[] ratingTrend = new float[1];
                ratingTrend[0] = 0.0f;

                List<MatchSession> sessions;

                if (marathonTable)
                    sessions = await db.MatchSessions.Include("Matches.Goals").Include("Team1.Players").Include("Team2.Players").Where(x => (x.Team1.Players.Any(y => y.PlayerID == p.PlayerID) || x.Team2.Players.Any(y => y.PlayerID == p.PlayerID))).ToListAsync<MatchSession>();
                else
                    sessions = await db.MatchSessions.Include("Matches.Goals").Include("Team1.Players").Include("Team2.Players").Where(x => (x.Season.SeasonID == model.Season.SeasonID || marathonTable) && (x.Team1.Players.Any(y => y.PlayerID == p.PlayerID) || x.Team2.Players.Any(y => y.PlayerID == p.PlayerID))).ToListAsync<MatchSession>();

                foreach (MatchSession ms in sessions)
                {
                    matchSessionCount++;

                    foreach(Match m in ms.Matches)
                    {
                        matchCount++;
                        goalCount += m.Goals.Where(x => x.Scorer.PlayerID == p.PlayerID).Count();
                        int goalsTeam1 = m.Goals.Where(x => ms.Team1.Players.Any(y => y.PlayerID == x.Scorer.PlayerID)).Count();
                        int goalsTeam2 = m.Goals.Where(x => ms.Team2.Players.Any(y => y.PlayerID == x.Scorer.PlayerID)).Count();

                        bool playerInTeam1 = ms.Team1.Players.Any(x => x.PlayerID == p.PlayerID);
                        bool playerInTeam2 = ms.Team2.Players.Any(x => x.PlayerID == p.PlayerID);

                        if (playerInTeam1)
                        {
                            goalsAgainst += goalsTeam2;
                            teamGoalCount += goalsTeam1;
                            if(goalsTeam1 > goalsTeam2)
                                wonMatchesCount++;
                        }
                        else if (playerInTeam2)
                        {
                            goalsAgainst += goalsTeam1;
                            teamGoalCount += goalsTeam2;
                            if(goalsTeam2 > goalsTeam1)
                                wonMatchesCount++;
                        }
                            

                    }

                    // Calculate intermediate rating
                    /*
                    int currentMatchSessionCount = db.MatchSessions.Where(x => x.MatchDate <= ms.MatchDate && x.Season.SeasonID == ms.Season.SeasonID).Count();
                    float currentRating = ((wonMatchesCount - (matchCount - wonMatchesCount)) + (0.5f * matchSessionCount) + (0.1f * goalCount)) * ((float)matchSessionCount / (float)currentMatchSessionCount);

                    for(int i = currentMatchSessionCount; i < ratingTrend.Length; i++)
                    {
                        ratingTrend[i] = currentRating;
                    }

                    if (lastParticipatingMatchSession == 0)
                    {
                        lastParticipatingMatchSession = currentMatchSessionCount;
                        for(int i = 0; i < lastParticipatingMatchSession; i++)
                        {
                            ratingTrend[i] = 0;
                        }
                    }
                    else
                    {

                    }
                    */
                
                }

                if (matchSessionCount > 0)
                {
                    rows.Add(
                        new PlayerStatsRow()
                        {
                            Player = p,
                            TotalMatchSessionCount = model.TotalMatchSessionCount,
                            MatchSessionCount = matchSessionCount,
                            MatchCount = matchCount,
                            MatchesWonCount = wonMatchesCount,
                            GoalsScored = goalCount,
                            GoalsAgainst = goalsAgainst,
                            TeamGoals = teamGoalCount,
                            RatingTrend = ratingTrend.ToList<float>()
                        });
                }
            }

            model.PlayerStatsRows = rows;

            return View(model);
        }
    }
}