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

        // Post: SeletElement
        [HttpPost]
        public ActionResult Index(int siteId)
        {
            ViewBag.siteId = siteId;

            RecruitingSite site = null;
            IQueryable<RecruitingSite> query = from s in db.RecruitingSites
                                 where (s.ID == siteId)
                                select s;

            if ( query  != null )
            {
                site = query.FirstOrDefault<RecruitingSite>();
            }


            return View(site);
        }
    }
}