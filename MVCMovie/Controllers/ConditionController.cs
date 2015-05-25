using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public void SetCondition(ConditionViewModel condViewModel)
        {
            if (condViewModel == null)
            {
                return;
            }

            int condID = condViewModel.ID;
            List<string> titleConds = condViewModel.titleConds;
            List<string> locationConds = condViewModel.locationConds;

            if (titleConds == null && locationConds == null)
            {
                return;
            }


            if (ModelState.IsValid)
            {

                Condition condition = new Condition();
                condition.ID = condID;
                condition.titleConds = new List<TitleCond>();
                condition.locationConds = new List<LocationCond>();

                var qry = from s in db.Conditions
                          select s;
                qry = qry.Where(s => s.ID == condID);
                Condition cond = qry.FirstOrDefault();

                //The condition of condID doesn't exist, and then insert the new condition with the condID
                //Othewise, remove the original one and insert the new condition with the condID
                if (cond != null)
                {
                    db.Conditions.Remove(cond);
                    db.SaveChanges();           //Have to save change after remove. Otherwise, remove option will most likely be lost
                    db.Conditions.Add(condition);
                }
                else
                {
                    db.Conditions.Add(condition);
                }
                db.SaveChanges();

                //Re-read the condition of the condID and then set its titleConds and locationConds
                qry = qry.Where(s => s.ID == condID);
                cond = qry.FirstOrDefault();

                if (titleConds != null)
                {
                    for (int i = 0; i < titleConds.Count; i++)
                    {
                        TitleCond tc = new TitleCond();
                        tc.titleCond = titleConds.ElementAt(i);
                        cond.titleConds.Add(tc);
                    }
                }

                if (locationConds != null)
                {
                    for (int i = 0; i < locationConds.Count; i++)
                    {
                        LocationCond lc = new LocationCond();
                        lc.locationCond = locationConds.ElementAt(i);
                        cond.locationConds.Add(lc);
                    }
                }

                db.SaveChanges();
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

            List<TitleCond> titleConds = new List<TitleCond>();
            List<LocationCond> locationConds = new List<LocationCond>();
            foreach (TitleCond tc in cond.titleConds)
            {

                titleConds.Add(tc);
            }

            foreach (LocationCond lc in cond.locationConds)
            {

                locationConds.Add(lc);
            }

            Condition condition = new Condition();
            condition.titleConds = titleConds;
            condition.locationConds = locationConds;

            return Json(condition, JsonRequestBehavior.AllowGet);

        }
    }
}