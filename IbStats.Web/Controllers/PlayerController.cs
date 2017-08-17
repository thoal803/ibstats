using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IbStats.Models;

namespace IbStats.Controllers
{
    [Authorize]
    public class PlayerController : IbStatsController
    {
        // GET: Player
        public ActionResult Index()
        {
            return View(db.Players.ToList<Player>());
        }

        public ActionResult Edit(int id)
        {
            Player p = null;
            if (id == 0)
            {
                p = new Player();
            }
            else
            {
                p = db.Players.Where(x => x.PlayerID == id).FirstOrDefault();
            }

            return View(p);
        }
        public ActionResult Delete(int id)
        {
            Player p = null;
            p = db.Players.Where(x => x.PlayerID == id).FirstOrDefault();

            if (p != null)
            {
                db.Players.Remove(p);
                db.SaveChanges();
                
            }
            return RedirectToAction("Index", "Player");

        }


        public ActionResult Save(Player p)
        {
            
            if(p.PlayerID == 0)
            {
                db.Players.Add(p);
            }
            else
            {
                Player oldPlayer = db.Players.Where(x => x.PlayerID == p.PlayerID).FirstOrDefault();
                if (oldPlayer != null)
                {
                    oldPlayer.Name = p.Name;
                    oldPlayer.Email = p.Email;
                }

            }
            

            db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}