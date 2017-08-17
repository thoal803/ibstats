using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IbStats.ViewModels;
using IbStats.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IbStats.Controllers
{
    [Authorize]
    public class GameController : IbStatsController
    {
        // GET: Game
        public async Task<ActionResult> Index(int? seasonID)
        {
            GameViewModel model = new GameViewModel();
            model.AllSeasons = await GetAllSeasonsAsync();

            if (seasonID == null)
                model.Season = await GetCurrentSeasonAsync();
            else
                model.Season = await db.Seasons.Where(x => x.SeasonID == seasonID).FirstOrDefaultAsync<Season>();

            model.MatchSessions = await db.MatchSessions.Where(x => x.Season.SeasonID == model.Season.SeasonID).ToListAsync<MatchSession>();
            return View(model);
        }

        public ActionResult PlayMatchSession(int id, int? matchId)
        {
            PlayMatchViewModel model = new PlayMatchViewModel();
            model.MatchSession = db.MatchSessions.Where(m => m.MatchSessionID == id).Include("Matches.Goals").Include("Team1.Players").Include("Team2.Players").FirstOrDefault();
            model.CurrentMatch = db.Matches.Where(m => m.MatchID == matchId).Include(m => m.Goals).FirstOrDefault();
            
            if(model.CurrentMatch == null)
            {
                model.MatchList = model.MatchSession.Matches;
            }
            else
            {
                model.MatchList = model.MatchSession.Matches.Where(x => x.MatchID != model.CurrentMatch.MatchID).ToList();
            }

            return View(model);
        }

        public ActionResult StartMatch(int id)
        {
            MatchSession ms = db.MatchSessions.Where(m => m.MatchSessionID == id).Include(m => m.Matches).FirstOrDefault();

            Match match = new Match() { StartTime = DateTime.Now };
            ms.Matches.Add(match);

            db.SaveChanges();

            return RedirectToAction("PlayMatchSession", "Game", new { id = id, matchId = match.MatchID });
        }
        public ActionResult EndMatch(int id, int matchId)
        {
            Match m = db.Matches.Where(x => x.MatchID == matchId).FirstOrDefault<Match>();
            if(m != null)
            {
                m.EndTime = DateTime.Now;
                db.SaveChanges();
            }

            return RedirectToAction("PlayMatchSession", "Game", new { id = id });
        }


        public ActionResult DeleteMatch(int id, int matchId, int? currentMatchId)
        {
            MatchSession ms = db.MatchSessions.Where(m => m.MatchSessionID == id).Include(m => m.Matches).FirstOrDefault();

            Match match = db.Matches.Where(m => m.MatchID == matchId).Include(m => m.Goals).FirstOrDefault();

            db.Goals.RemoveRange(match.Goals);

            ms.Matches.Remove(match);
            db.Matches.Remove(match);

            db.SaveChanges();

            object routeValues = null;
            if (currentMatchId == null)
                routeValues = new { id = id };
            else
                routeValues = new { id = id, matchId = currentMatchId };

            return RedirectToAction("PlayMatchSession", "Game", routeValues);
        }

        public ActionResult AddGoal(int id, int matchId, int playerId)
        {
            Match m = db.Matches.Where(x => x.MatchID == matchId).Include(x => x.Goals).FirstOrDefault();
            Player p = db.Players.Where(x => x.PlayerID == playerId).FirstOrDefault();

            m.Goals.Add(new Goal() { Scorer = p, RegistrationTime = DateTime.Now });

            db.SaveChanges();

            return RedirectToAction("PlayMatchSession", "Game", new { id = id, matchId = matchId });
        }

        public ActionResult DeleteLastGoal(int id, int matchId)
        {
            Match m = db.Matches.Where(x => x.MatchID == matchId).Include(x => x.Goals).FirstOrDefault();
            Goal lastGoal = m.Goals.OrderByDescending(x => x.RegistrationTime).FirstOrDefault();

            if (lastGoal != null)
            {
                m.Goals.Remove(lastGoal);
                db.Goals.Remove(lastGoal);
                db.SaveChanges();
            }

            return RedirectToAction("PlayMatchSession", "Game", new { id = id, matchId = matchId });
        }

        public ActionResult EditMatch(int id, int matchId, int? currentMatchId)
        {
            EditMatchViewModel model = new EditMatchViewModel();
            model.MatchSessionID = id;
            model.CurrentMatchID = currentMatchId ?? 0;
            model.Match = db.Matches.Where(x => x.MatchID == matchId).Include(x => x.Goals.Select(y => y.Scorer)).FirstOrDefault();

            return View(model);
        }
    } 
}