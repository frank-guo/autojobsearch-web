using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MVCMovie.Models;
using MVCMovie.Services;

namespace MVCMovie.Controllers
{
    public class ConditionController : Controller
    {
        private IConditionService conditionService;

        public ConditionController(IConditionService conditionService)
        {
            this.conditionService = conditionService;
        }

        // GET: Condition
        public ActionResult Index(int id)
        {
            ViewBag.siteId = id;
            return View();
        }

        [HttpPost]
        public ActionResult SetCondition(ConditionViewModel condViewModel)
        {
            if (condViewModel == null && condViewModel.ID <= 0)
            {
                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.InvlidSiteID,
                    message = Resources.Common.InvlidSiteID,
                };
                return Json(response);
            }

            if (ModelState.IsValid)
            {
                conditionService.Update(condViewModel);

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
            var condition = conditionService.GetById(siteId);

            return Json(condition, JsonRequestBehavior.AllowGet);

        }
    }
}