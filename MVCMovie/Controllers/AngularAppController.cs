using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMovie.Controllers
{
    public class AngularAppController : Controller
    {
        // GET: AngularApp
        public ActionResult Index()
        {
            return View();
        }
    }
}