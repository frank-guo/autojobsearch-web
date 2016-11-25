using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using MVCMovie.Models;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using WatiN.Core;
using MVCMovie.Services;  


namespace MVCMovie.Controllers
{
    //This is to enhance the Watin Framework to have a children method for an element
    public static class WatinExtensions
    {
        public static ElementCollection Children(this Element self)
        {
            return self.DomContainer.Elements.Filter(e => self.Equals(e.Parent));
        }
    }

    public class URLTransformer : System.Windows.Forms.Form
    {
        private string url;
        private string baseUrl;

        public URLTransformer(string url)
        {
            this.url = url;
            this.baseUrl = getBaseUrl(url);
        }

        public void transformUrl(ref string webpage)
        {
            if (url == null)
            {
                return;
            }
            
            WebBrowser browser = new WebBrowser();
            WebClient client = new WebClient();
            browser.ScriptErrorsSuppressed = true;
            browser.Url = new Uri(Resources.Common.BlankUrl);

            string html;

            try { 
                html = client.DownloadString(url);
            }
            catch
            {
                html = null;
            }
            HtmlDocument document = browser.Document.OpenNew(true);
            document.Write(html);
            HtmlElement htmlElement = document.GetElementsByTagName("html")[0];

            tranverse(htmlElement);

            webpage = htmlElement.InnerHtml;
        }



        private void tranverse(HtmlElement root)
        {
            if (root == null)
            {
                return;
            }

            HtmlElementCollection elmts = root.Children;
            // ToDo: elements[0] is root, [1] is head, [2] is title....
            //DomContainer has child method. Could take a look at the source code of child

            HtmlElement e = null;
            if (elmts.Count != 0) 
            {
                e = elmts[0];
            }

            while(e != null)
            {
                tranverse(e);
                e = e.NextSibling;
            }

            transformUrl(ref root);

        }


        //Transform any attr with a relative url of this element to the absolute url
        private void transformUrl(ref HtmlElement e)
        {
            string urlOfElement = null;
            string baseUrl = getBaseUrl(url);
            string attrName = null;            
            
            if (e.TagName == "SCRIPT")
            {
                urlOfElement = e.GetAttribute("src");
                attrName = "src";
            }
            if (e.TagName == "LINK")
            {
                urlOfElement = e.GetAttribute("href");
                attrName = "href";
            }

            if (e.TagName == "A")
            {
                urlOfElement = e.GetAttribute("href");
                attrName = "href";
            }

            if (e.TagName == "IMG")
            {
                urlOfElement = e.GetAttribute("src");
                attrName = "src";
            }

            if (e.TagName == "IFRAME")
            {
                urlOfElement = e.GetAttribute("src");
                attrName = "src";
            }

            if (urlOfElement!=null && urlOfElement.StartsWith("about:/"))
            {
                urlOfElement = urlOfElement.Substring(6);
            }

            //the attr of src or href could be "" even though the element doesn't have the attr
            if (urlOfElement != null && urlOfElement != "" && !urlOfElement.StartsWith("http") 
                && !urlOfElement.StartsWith("//") && !urlOfElement.StartsWith("https"))
            {
                urlOfElement = baseUrl + urlOfElement;
                e.SetAttribute(attrName, urlOfElement);
            }                             
        }

        private string getBaseUrl(string url)
        {
            string baseUrl = null;

            if (url != null && url.StartsWith("http"))
            {
                int count = 0;
                int j = 0;

                for (; j <= url.Length - 1 && count < 3; j++)
                {

                    if (url.ElementAt<char>(j) == '/')
                    {
                        count++;
                    }
                }

                //At this point, j points to the char right after the third '/' or j = url.length
                //j-1 represents the length of the substring ahead of the third '/'
                baseUrl = url.Substring(0, j - 1);
            }

            return baseUrl;
        }
    }

    public class BrowserController : Controller
    {
        private RecruitingSiteDBContext db = new RecruitingSiteDBContext();
        private IRecruitingSiteService recruitingSiteService;
        private IPathNodeService pathNodeService;
        private IJob2PathService job2PathService;
        private ICompanyPathService companyPathService;
        private IOthersPathService othersPathService;
        private INextPathService nextPathService;

        private const int defaultSiteID = 1;
        private const int InvalidSiteID = 0;

        public BrowserController(IRecruitingSiteService recruitingSiteService,
            IPathNodeService pathNodeService, IJob2PathService job2PathServices,
            ICompanyPathService companyPathService, IOthersPathService othersPathService,
            INextPathService nextPathService)
        {
            this.pathNodeService = pathNodeService;
            this.job2PathService = job2PathServices;
            this.companyPathService = companyPathService;
            this.othersPathService = othersPathService;
            this.nextPathService = nextPathService;
            this.recruitingSiteService = recruitingSiteService;
        }

        //[HttpPost]
        //public int SetURL(int id, string url)
        //{
        //    if (url == null || url == "")
        //    {
        //        return InvalidSiteID;
        //    }

        //    RecruitingSite site = new RecruitingSite();
        //    site.url = url;

        //    if (ModelState.IsValid)
        //    {
        //        var sites = new List<RecruitingSite>();
        //        var qry = from s in db.RecruitingSites
        //                  select s;
        //        qry = qry.Where(u => u.url == url);
        //        sites.AddRange(qry.Distinct());
        //        if (sites.Count == 0)    //check if the url site does not exists already
        //        {
        //            RecruitingSite returnedSite = db.RecruitingSites.Add(site);
        //            db.SaveChanges();
        //            return returnedSite.ID;
        //        }
        //        else
        //        {
        //            return sites[0].ID;     //If the url exists, return its ID
        //        }
        //    }

        //    return InvalidSiteID;
        //}

        [HttpPost]
        public void SetNext(List<int> listNextPositions, int id)
        {
            if (listNextPositions == null || id <= 0)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                nextPathService.Update(listNextPositions, id);
            }
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

        [HttpPost]
        public void SetCompany(List<int> listCompanyPositions, int id)
        {
            if (listCompanyPositions == null || id <= 0)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                companyPathService.Update(listCompanyPositions, id);
            }
        }

        public JsonResult GetCompany(int siteId)
        {

            IList<Company> companyPath = companyPathService.Get(siteId);

            List<int> listCompanyPositions = new List<int>();
            foreach (Company p in companyPath)
            {

                listCompanyPositions.Add(p.position);
            }

            return Json(listCompanyPositions, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void SetOthers(List<int> othersPath, int id)
        {
            if (othersPath == null || id <= 0)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                othersPathService.Update(othersPath, id);
            }
        }

        public JsonResult GetOthers(int siteId)
        {

            var qry = from s in db.RecruitingSites
                      select s;
            qry = qry.Where(s => s.ID == siteId);
            RecruitingSite site = qry.FirstOrDefault();

            List<int> listOthersPositions = new List<int>();
            foreach (Others p in site.othersPath)
            {

                listOthersPositions.Add(p.position);
            }

            return Json(listOthersPositions, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetNext(int siteId)
        {

            var qry = from s in db.RecruitingSites
                        select s;
            qry = qry.Where(s => s.ID == siteId);
            RecruitingSite site = qry.FirstOrDefault();

            List<int> listNextPositions = new List<int>();
            foreach (NextPosition p in site.ListNextPositions)
            {

                listNextPositions.Add(p.position);
            }

            return Json(listNextPositions, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetUrls()
        {

            var result = from s in db.RecruitingSites
                            select new WebsiteViewModel 
                            { 
                                ID = s.ID,
                                siteName = s.siteName,
                                url = s.url
                            };
            List<WebsiteViewModel> sites = result.ToList();

            return Json(sites, JsonRequestBehavior.AllowGet);

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

        [HttpPost]
        public void SetJob1(IList<PathNode> job1Path, int id)
        {
 
            if (job1Path == null)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                pathNodeService.Update(job1Path, id);
            }
        }

        [HttpPost]
        public void SetJob2(List<int> job2Path, int id)
        {
            if (job2Path == null || id <= 0)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                job2PathService.Update(job2Path, id);
            }
        }

        public JsonResult GetJob2(int siteId)
        {

            IList<Job2Position> job2Path = job2PathService.Get(siteId);

            List<int> listPositions = new List<int>();
            foreach (Job2Position p in job2Path)
            {

                listPositions.Add(p.position);
            }

            return Json(listPositions, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void SetLevelNo(int levelNoLinkHigherJob1, int siteId)
        {
            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(s => s.ID == siteId);
                RecruitingSite site = qry.FirstOrDefault();

                site.levelNoLinkHigherJob1 = levelNoLinkHigherJob1;
                db.SaveChanges();
            }
        }

        public JsonResult GetLevelNo(int siteId)
        {
            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          where (s.ID == siteId)
                          select s.levelNoLinkHigherJob1;
                int levelNoLinkHigherJob1 = qry.FirstOrDefault();
                return Json(levelNoLinkHigherJob1);
            }

            return null;
        }

        [HttpPost]
        public void SetIsContainJobLink(bool isContainJobLink)
        {
            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(s => s.ID == defaultSiteID);
                RecruitingSite site = qry.FirstOrDefault();

                site.isContainJobLink = isContainJobLink;
                db.SaveChanges();
            }
        }

        public JsonResult GetIsContainJobLink(int siteId)
        {
            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          where (s.ID == siteId)
                          select s.isContainJobLink;
                bool isContainJobLink = qry.FirstOrDefault();
                return Json(isContainJobLink);
            }

            return null;
        }

        [HttpPost]
        public JsonResult GetJob1(int siteId)
        {

            if (siteId <= 0)
            {
                return null;
            }

            IList<PathNode> job1Path = pathNodeService.Get(siteId);

            List<PathNode> jobsPath = new List<PathNode>();
            foreach (PathNode p in job1Path)
            {
                PathNode pn = new PathNode();
                pn.position = p.position;
                pn.hasCommonParent = p.hasCommonParent;
                jobsPath.Add(pn);
            }

            return Json(jobsPath, JsonRequestBehavior.AllowGet);

        }

        public void DeleteAllJobSetting(int siteId)
        {
            if (siteId <= 0)
            {
                return;
            }
            RecruitingSite site = recruitingSiteService.GetByID(siteId);
            site.levelNoLinkHigherJob1 = 0;
            site.isContainJobLink = false;
            recruitingSiteService.Update(site);

            companyPathService.Delete(site.companyPath);
            job2PathService.Delete(site.Job2Path);
            pathNodeService.Delete(site.JobPath);            
            othersPathService.Delete(site.othersPath);
            nextPathService.Delete(site.ListNextPositions);
            

        }

        //? represent id could be null 
        //the parameter name has to be id which is matching the name of id in the routeconfig.cs
        //Otherwise, MVC would not know which parameter the third segment of url should match if there are multiple parameters 
        public ActionResult Index(int? id)
        {

            if (id == null || id == 0)
            {
                return null;
            }
            
            string webaddress = null;
            WebClient client = new WebClient();

            //get the first site url
            var siteUrls = new List<string>();
            var qry = from s in db.RecruitingSites
                      where(s.ID == id)
                       select s.url;
            siteUrls.AddRange(qry.Distinct());

            webaddress = siteUrls[0];
            string webpageUpdate = null;

            URLTransformer urlTranformer = new URLTransformer(webaddress);

            //In order to get htmlDocument to navigate easily on the webpage, use WebBrowser class
            //Therefore, an sepearate thread is created to build up a WebBrowser
            var t = new Thread(() => urlTranformer.transformUrl(ref webpageUpdate));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            while (t.IsAlive)
            {
                Thread.Sleep(1000);
            }

            IframViewModel iframe = new IframViewModel();
            iframe.webpageUpdate = webpageUpdate;
            //IframViewModel is originally created to pass the siteId, ie. id, to the view file of iframe
            //but the simple approach to get this id in javascript code of ContextMenu.js has not been come up with.
            //The way to put script tags encolsing the code to get this id directly in the view file probably
            //because @Html.Raw(@Model.webpageUpdate) contains <html> already.
            //Eventually use a workaround to get this id from the source of <myframe>
            iframe.ID = (int)id;

            return View(iframe);

        }

    }


}