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
using System.Collections;
using MVCMovie.Repositories;
using MVCMovie.Services;

namespace MVCMovie.Controllers
{
    [Authorize(Roles = "Regular")]
    public class HomeController : Controller
    {
        private IRecruitingSiteService recruitingSiteService;

        public HomeController(IRecruitingSiteService service)
        {
            this.recruitingSiteService = service;
        }

        // GET: Sites
        public ActionResult Index()
        {
            //var sites = siteDb.RecruitingSites.Select(s => new
            //{
            //    s.siteName,
            //    s.url,
            //    s.ID
            //}).ToList();

            //var options = new List<WebsiteViewModel>();
            //foreach(var site in sites) {
            //    WebsiteViewModel webSiteVM = new WebsiteViewModel();
            //    webSiteVM.ID = site.ID;
            //    string siteName = site.siteName;
            //    webSiteVM.url = siteName == null || siteName == "" ? site.url : site.siteName + ": " + site.url;
            //    options.Add(webSiteVM);
            //}
            //ViewBag.options = new SelectList(options, "ID", "url");

            return View();
        }

        public JsonResult GetSites()
        {

            //var result = from s in siteDb.RecruitingSites
            //             select new WebsiteViewModel
            //             {
            //                 ID = s.ID,
            //                 siteName = s.siteName,
            //                 url = s.url
            //             };
            //List<WebsiteViewModel> sites = result.ToList();

            IEnumerable<WebsiteViewModel> sites = recruitingSiteService.Get();
            return Json(sites, JsonRequestBehavior.AllowGet);

        }

        //Create a new web site
        [HttpPost]
        public void CreateSite(string url, string siteName)
        {
            recruitingSiteService.CreateSite(url, siteName);

        }


        //update an existing web site
        [HttpPost]
        public void UpdateSite(int id, string url, string siteName)
        {
            recruitingSiteService.Update(id, url, siteName);
        }

        //Delete a web site
        public void DeleteSite(int id)
        {
            recruitingSiteService.Delete(id);
        }
    }
}
