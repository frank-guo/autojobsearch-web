using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMovie.Controllers
{
    public class SeletElementController : Controller
    {
        // Post: SeletElement
        [HttpPost]
        public ActionResult Index(int siteId)
        {
            ViewBag.siteId = siteId;
            return View();
        }
    }
}