using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using MVCMovie.Models;

namespace MVCMovie.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private RecruitingSiteDBContext siteDb = new RecruitingSiteDBContext();

        // GET: Sites
        public ActionResult Index()
        {
            var options = siteDb.RecruitingSites.Select(s => new
            {
                s.url,
                s.ID
            }).ToList();

            ViewBag.options = new SelectList(options, "ID", "url");

            return View();
        }

        //Create a new web site
        public ActionResult CreateSite(string url)
        {
            RecruitingSite newsite = new RecruitingSite();

            newsite.url = url;

            newsite = siteDb.RecruitingSites.Add(newsite);
            siteDb.SaveChanges();

            var sites = siteDb.RecruitingSites.Select(s => new
            {
                s.url,
                s.ID
            }).ToList();

            return Json(new { sites = sites, newID = newsite.ID }, JsonRequestBehavior.AllowGet);
        }

        //Delete a web site
        public ActionResult DeleteSite(int id)
        {

            var qry = from site in siteDb.RecruitingSites
                      select site;
            qry = qry.Where(s => s.ID == id);
            var retrievedSite = qry.FirstOrDefault();

            siteDb.RecruitingSites.Remove(retrievedSite);
            siteDb.SaveChanges();

            var sites = siteDb.RecruitingSites.Select(s => new
            {
                s.url,
                s.ID
            }).ToList();

            return Json(sites, JsonRequestBehavior.AllowGet);
        }
    }
}
