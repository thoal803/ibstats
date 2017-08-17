using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IbStats.Data;
using IbStats.Models;
using System.Threading.Tasks;
using System.Data.Entity;


namespace IbStats.Controllers
{
    public class IbStatsController : Controller
    {
        protected IbStatsContext db = new IbStatsContext();

        protected async Task<Season> GetCurrentSeasonAsync()
        {
            return await db.Seasons.Where(s => s.SeasonStart <= DateTime.Now && s.SeasonEnd >= DateTime.Now).FirstOrDefaultAsync<Season>();
        }

        protected async Task<List<Season>> GetAllSeasonsAsync()
        {
            return await db.Seasons.OrderBy(x => x.SeasonStart).ToListAsync<Season>();
        }

        protected async Task<List<Player>> GetAllPlayersAsync()
        {
            return await db.Players.OrderBy(x => x.Name).ToListAsync<Player>();
        }
    }
}