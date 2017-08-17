using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IbStats.Models;
using IbStats.ViewModels;
using System.Data.Entity;
using System.Threading.Tasks;


namespace IbStats.Controllers
{
    [Authorize]
    public class MatchSessionController : IbStatsController
    {
        // GET: MatchSession
        public async Task<ActionResult> Index(int? seasonID)
        {
            MatchSessionListViewModel model = new MatchSessionListViewModel();
            
            if (seasonID == null)
                model.Season = await GetCurrentSeasonAsync();
            else
                model.Season = await db.Seasons.Where(s => s.SeasonID == seasonID).FirstOrDefaultAsync();

            model.AllSeasons = await GetAllSeasonsAsync();

            model.MatchSessions = await db.MatchSessions.Include(t => t.Team1.Players).Include(t => t.Team2.Players).Include(x => x.FirstWasher).Include(x => x.SecondWasher).Where(m => m.Season.SeasonID == model.Season.SeasonID).ToListAsync<MatchSession>();
            
            return View(model);
        }

        public async Task<ActionResult> Edit(int id, int? seasonID)
        {
            MatchSession ms = null;
            if (id == 0)
            {
                ms = new MatchSession() { MatchDate = DateTime.Now };
                ms.Team1 = new Team() { Players = new List<Player>() };
                ms.Team2 = new Team() { Players = new List<Player>() };
                if (seasonID == null)
                    ms.Season = await GetCurrentSeasonAsync();
                else
                    ms.Season = await db.Seasons.Where(s => s.SeasonID == seasonID).FirstOrDefaultAsync();
            }
            else
            {
                ms = db.MatchSessions.Include("Matches.Goals").Include(t => t.Team1.Players).Include(t => t.Team2.Players).Include(t => t.FirstWasher).Include(t => t.SecondWasher).Where(x => x.MatchSessionID == id).FirstOrDefault();
            }

            return View(new MatchSessionViewModel() { MatchSession = ms, AllPlayers = await GetAllPlayersAsync(), AllSeasons = await GetAllSeasonsAsync() });
        }

        public ActionResult Save(MatchSessionViewModel m)
        {
            MatchSession ms = null;

            if(m.MatchSession.MatchSessionID == 0)
            {
                ms = m.MatchSession;

                ms.MatchDate = m.MatchSession.MatchDate;

                Team t1 = new Team() { Players = db.Players.Where(x => m.SelectedPlayerIdsTeam1.Contains(x.PlayerID)).ToList<Player>() };
                Team t2 = new Team() { Players = db.Players.Where(x => m.SelectedPlayerIdsTeam2.Contains(x.PlayerID)).ToList<Player>() };

                ms.Team1 = t1;
                ms.Team2 = t2;

                ms.FirstWasher = db.Players.Where(x => x.PlayerID == m.MatchSession.FirstWasher.PlayerID).FirstOrDefault();
                ms.SecondWasher = db.Players.Where(x => x.PlayerID == m.MatchSession.SecondWasher.PlayerID).FirstOrDefault();
                
                ms.Season = db.Seasons.Where(x => x.SeasonID == m.MatchSession.Season.SeasonID).FirstOrDefault();
                
                db.MatchSessions.Add(ms);
            }
            else
            {
                ms = db.MatchSessions.Where(x => x.MatchSessionID == m.MatchSession.MatchSessionID).Include("Team1.Players").Include("Team2.Players").Include(x => x.FirstWasher).Include(x => x.SecondWasher).FirstOrDefault();

                ms.MatchDate = m.MatchSession.MatchDate;

                if(ms.Team1.Players != null)
                    ms.Team1.Players.Clear();
                
                if (ms.Team2.Players != null)
                    ms.Team1.Players.Clear();

                ms.Team1.Players = db.Players.Where(x => m.SelectedPlayerIdsTeam1.Contains(x.PlayerID)).ToList<Player>();
                ms.Team2.Players = db.Players.Where(x => m.SelectedPlayerIdsTeam2.Contains(x.PlayerID)).ToList<Player>();

                ms.FirstWasher = db.Players.Where(x => x.PlayerID == m.MatchSession.FirstWasher.PlayerID).FirstOrDefault();
                ms.SecondWasher = db.Players.Where(x => x.PlayerID == m.MatchSession.SecondWasher.PlayerID).FirstOrDefault();

                ms.Season = db.Seasons.Where(x => x.SeasonID == m.MatchSession.Season.SeasonID).FirstOrDefault();


            }


            
            db.SaveChanges();

            return RedirectToAction("Index");   
        }

        public ActionResult Delete(int id)
        {
            MatchSession ms = db.MatchSessions.Where(m => m.MatchSessionID == id).Include(x => x.Team2).Include(x => x.Team1).Include(x => x.Matches).FirstOrDefault();

            
            if(ms != null)
            {
                db.Matches.RemoveRange(ms.Matches);

                db.MatchSessions.Remove(ms);
                
                db.SaveChanges();
            }

            return RedirectToAction("Index");   

        }

        public ActionResult DeleteMatch(int id, int matchId)
        {
            MatchSession ms = db.MatchSessions.Where(m => m.MatchSessionID == id).Include(m => m.Matches).FirstOrDefault();

            Match match = db.Matches.Where(m => m.MatchID == matchId).Include(m => m.Goals).FirstOrDefault();

            db.Goals.RemoveRange(match.Goals);

            ms.Matches.Remove(match);
            db.Matches.Remove(match);

            db.SaveChanges();

            return RedirectToAction("Edit", "MatchSession", new { id = id });
        }

        
    }
}