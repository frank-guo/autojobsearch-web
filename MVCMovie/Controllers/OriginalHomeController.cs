using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMovie.Controllers
{
    public class OriginalHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Job Hunting Tool";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My Contact.";

            return View();
        }
    }
}