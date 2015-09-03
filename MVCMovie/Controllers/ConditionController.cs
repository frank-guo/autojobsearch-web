using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MVCMovie.Models;

namespace MVCMovie.Controllers
{
    public class ConditionController : Controller
    {
        private RecruitingSiteDBContext db = new RecruitingSiteDBContext();

        // GET: Condition
        public ActionResult Index(int id)
        {
            ViewBag.siteId = id;
            return View();
        }

        [HttpPost]
        public ActionResult SetCondition(ConditionViewModel condViewModel)
        {
            if (condViewModel != null && condViewModel.ID <= 0)
            {
                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.InvlidSiteID,
                    message = Resources.Common.InvlidSiteID,
                };
                return Json(response);
            }

            var condID = condViewModel.ID;
            var titleConds = condViewModel.titleConds;
            var locationConds = condViewModel.locationConds;

            if (ModelState.IsValid)
            {

                var condition = new Condition();
                condition.ID = condID;
                condition.titleConds = new List<TitleCond>();
                condition.locationConds = new List<LocationCond>();

                var qry = from s in db.Conditions
                          select s;
                qry = qry.Where(s => s.ID == condID);
                var cond = qry.FirstOrDefault();

                //The condition of condID doesn't exist, and then insert the new condition with the condID
                //Othewise, remove the original one and insert the new condition with the condID
                if (cond != null)
                {
                    db.Conditions.Remove(cond);
                    db.SaveChanges();           //Have to save change after remove. Otherwise, remove option will most likely be lost
                }
                db.Conditions.Add(condition);
                db.SaveChanges();

                //Re-read the condition of the condID and then set its titleConds and locationConds
                qry = qry.Where(s => s.ID == condID);
                cond = qry.FirstOrDefault();

                if (titleConds != null)
                {
                    for (int i = 0; i < titleConds.Count; i++)
                    {
                        var tc = new TitleCond
                        {
                            titleCond = titleConds.ElementAt(i)
                        };
                        if (cond != null) 
                            cond.titleConds.Add(tc);
                    }
                }

                if (locationConds != null)
                {
                    for (int i = 0; i < locationConds.Count; i++)
                    {
                        var lc = new LocationCond
                        {
                            locationCond = locationConds.ElementAt(i)
                        };
                        if (cond != null) cond.locationConds.Add(lc);
                    }
                }

                db.SaveChanges();

                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.Success,
                    message = String.Format(Resources.Common.SaveSuccessMsg, Resources.Condition.condition),
                };
                return Json(response);
            }
            else
            {
                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.InvlidModel,
                    message = String.Format(Resources.Common.InvlidModel, Resources.Condition.condition),
                };
                return Json(response);
            }
        }

        public JsonResult GetCondition(int siteId)
        {

            var qry = from s in db.Conditions
                      select s;
            qry = qry.Where(s => s.ID == siteId);
            Condition cond = qry.FirstOrDefault();

            if (cond == null)
            {
                return null;
            }

            var titleConds = new List<TitleCond>();
            var locationConds = new List<LocationCond>();
            foreach (TitleCond tc in cond.titleConds)
            {

                titleConds.Add(tc);
            }

            foreach (LocationCond lc in cond.locationConds)
            {

                locationConds.Add(lc);
            }

            var condition = new Condition();
            condition.titleConds = titleConds;
            condition.locationConds = locationConds;

            return Json(condition, JsonRequestBehavior.AllowGet);

        }
    }
}