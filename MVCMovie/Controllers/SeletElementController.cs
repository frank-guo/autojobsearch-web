using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCMovie.Models;
using MVCMovie.Services;

namespace MVCMovie.Controllers
{
    public class SeletElementController : Controller        
    {

        private IRecruitingSiteService recruitingSiteService;

        public SeletElementController(IRecruitingSiteService service)
        {
            this.recruitingSiteService = service;
        }

        public ActionResult Index(int id)
        {
            ViewBag.siteId = id;
            RecruitingSite site = recruitingSiteService.GetByID(id);
            return View(site);
        }
    }
}