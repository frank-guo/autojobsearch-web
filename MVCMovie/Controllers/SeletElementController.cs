using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCMovie.Models;

namespace MVCMovie.Controllers
{
    public class SeletElementController : Controller        
    {

        private RecruitingSiteDBContext db = new RecruitingSiteDBContext();

        public ActionResult Index(int id)
        {
            ViewBag.siteId = id;

            RecruitingSite site = null;
            IQueryable<RecruitingSite> query = from s in db.RecruitingSites
                                 where (s.ID == id)
                                select s;

            if ( query  != null )
            {
                site = query.FirstOrDefault<RecruitingSite>();
            }


            return View(site);
        }
    }
}