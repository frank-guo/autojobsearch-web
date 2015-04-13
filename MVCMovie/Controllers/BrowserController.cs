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
            browser.Url = new Uri("about:blank");

            string html;
            html = client.DownloadString(url);
            HtmlDocument document = browser.Document.OpenNew(true);
            document.Write(html);
            HtmlElement htmlElement = document.GetElementsByTagName("html")[0];

            /*
            Browser browserInstance;
            browserInstance = new IE(url);
            browserInstance.WaitForComplete();
            Element body = browserInstance.Body;
            Element htmlElmt = body.Parent;
            
            Element root = browserInstance.DomContainer.Elements[0];
             * */



            tranverse(htmlElement);

            /*
            HtmlElementCollection headChildren = htmlElement.GetElementsByTagName("head")[0].Children;
            foreach (HtmlElement e in headChildren)
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

                if(urlOfElement != null && urlOfElement!="" && !urlOfElement.StartsWith("http"))
                {
                    urlOfElement = baseUrl + urlOfElement;
                    e.SetAttribute(attrName,urlOfElement);
                }                             
            }
            */

            webpage = htmlElement.InnerHtml;
            //webpage = body.Parent.OuterHtml;

        }



        private void tranverse(HtmlElement root)
        {
            if (root == null)
            {
                return;
            }

            //ElementCollection children = root.Children();

            //foreach (Element e in children)
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
        private const int defaultSiteID = 1;
        private const int InvalidSiteID = 0;

        [HttpPost]
        public int SetURL(int id, string url)
        {
            if (url == null || url == "")
            {
                return InvalidSiteID;
            }

            RecruitingSite site = new RecruitingSite();
            site.url = url;

            if (ModelState.IsValid)
            {
                var sites = new List<RecruitingSite>();
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(u => u.url == url);
                sites.AddRange(qry.Distinct());
                if (sites.Count == 0)    //check if the url site does not exists already
                {
                    RecruitingSite returnedSite = db.RecruitingSites.Add(site);
                    db.SaveChanges();
                    return returnedSite.ID;
                }
                else
                {
                    return sites[0].ID;     //If the url exists, return its ID
                }
            }

            return InvalidSiteID;
        }

        [HttpPost]
        public void SetNext(List<int> listNextPositions)
        {
            if (listNextPositions == null)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                                      select s;
                qry = qry.Where(s => s.ID == 1);
                RecruitingSite site = qry.FirstOrDefault();

                for (int i=0; i<listNextPositions.Count;i++)
                {

                    if (site.ListNextPositions == null ||  site.ListNextPositions.Count-1 < i)
                    {
                        NextPosition np = new NextPosition();
                        np.position = listNextPositions.ElementAt(i);
                        site.ListNextPositions.Add(np);
                    }
                    else
                    {
                        site.ListNextPositions.ElementAt(i).position = listNextPositions.ElementAt(i);
                    }
                }
                db.SaveChanges();
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
                

                /*
                 var qry1 = from s in db.Conditions
                           where (s.ID == 1)
                           select s.locationConds;
                IList<LocationCond> lc1 = qry1.FirstOrDefault();
                 */

                //The condition of condID doesn't exist, and then insert the new condition with the condID
                //Othewise, remove the original one and insert the new condition with the condID
                if (cond != null)
                {
                    db.Conditions.Remove(cond);
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

                for (int i = 0; i < titleConds.Count; i++)
                {
                        TitleCond tc = new TitleCond();
                        tc.titleCond = titleConds.ElementAt(i);
                        cond.titleConds.Add(tc);
                }

                for (int i = 0; i < locationConds.Count; i++)
                {
                    LocationCond lc = new LocationCond();
                    lc.locationCond = locationConds.ElementAt(i);
                    cond.locationConds.Add(lc);
                }

                db.SaveChanges();
            }
        }

        [HttpPost]
        public void SetCompany(List<int> listCompanyPositions)
        {
            if (listCompanyPositions == null)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(s => s.ID == 1);
                RecruitingSite site = qry.FirstOrDefault();

                for (int i = 0; i < listCompanyPositions.Count; i++)
                {

                    if (site.companyPath == null || site.companyPath.Count - 1 < i)
                    {
                        Company cp = new Company();
                        cp.position = listCompanyPositions.ElementAt(i);
                        site.companyPath.Add(cp);
                    }
                    else
                    {
                        site.companyPath.ElementAt(i).position = listCompanyPositions.ElementAt(i);
                    }
                }
                db.SaveChanges();
            }
        }

        public JsonResult GetCompany()
        {

            var qry = from s in db.RecruitingSites
                      select s;
            qry = qry.Where(s => s.ID == 1);
            RecruitingSite site = qry.FirstOrDefault();

            List<int> listCompanyPositions = new List<int>();
            foreach (Company p in site.companyPath)
            {

                listCompanyPositions.Add(p.position);
            }

            return Json(listCompanyPositions, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void SetOthers(List<int> listOthersPositions)
        {
            if (listOthersPositions == null)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(s => s.ID == 1);
                RecruitingSite site = qry.FirstOrDefault();

                for (int i = 0; i < listOthersPositions.Count; i++)
                {

                    if (site.othersPath == null || site.othersPath.Count - 1 < i)
                    {
                        //Create new others and add to othersPath if there is no othersPath
                        //or the current othersPath is shorter than the new one
                        Others op = new Others();
                        op.position = listOthersPositions.ElementAt(i);
                        site.othersPath.Add(op);
                    }
                    else
                    {
                        //Overwrite the current othersPath
                        site.othersPath.ElementAt(i).position = listOthersPositions.ElementAt(i);
                    }
                }
                db.SaveChanges();
            }
        }

        public JsonResult GetOthers()
        {

            var qry = from s in db.RecruitingSites
                      select s;
            qry = qry.Where(s => s.ID == 1);
            RecruitingSite site = qry.FirstOrDefault();

            List<int> listOthersPositions = new List<int>();
            foreach (Others p in site.othersPath)
            {

                listOthersPositions.Add(p.position);
            }

            return Json(listOthersPositions, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetNext()
        {

            var qry = from s in db.RecruitingSites
                        select s;
            qry = qry.Where(s => s.ID == 1);
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
                                url = s.url
                            };
            List<WebsiteViewModel> sites = result.ToList();

            return Json(sites, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void SetJobs(List<PathNode> listPathNodes)
        {
 
            if (listPathNodes == null)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(s => s.ID == 1);
                RecruitingSite site = qry.FirstOrDefault();

                for (int i = 0; i < listPathNodes.Count; i++)
                {

                    if (site.JobPath == null || site.JobPath.Count - 1 < i)
                    {
                        PathNode pn = new PathNode();
                        pn.position = listPathNodes.ElementAt(i).position;
                        pn.hasCommonParent = listPathNodes.ElementAt(i).hasCommonParent;
                        site.JobPath.Add(pn);
                    }
                    else
                    {
                        site.JobPath.ElementAt(i).position = listPathNodes.ElementAt(i).position;
                        site.JobPath.ElementAt(i).hasCommonParent = listPathNodes.ElementAt(i).hasCommonParent;
                    }
                }
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void SetJob2(List<int> listJob2Positions, int siteId)
        {
            if (listJob2Positions == null)
            {
                return;
            }

            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          where (s.ID == siteId)
                          select s;
                RecruitingSite site = qry.FirstOrDefault();

                for (int i = 0; i < listJob2Positions.Count; i++)
                {
                    //If Job2Path is null  or the current Job2Path is already shorter than i
                    if (site.Job2Path == null || site.Job2Path.Count - 1 < i)
                    {
                        Job2Position j2p = new Job2Position();
                        j2p.position = listJob2Positions.ElementAt(i);
                        site.Job2Path.Add(j2p);
                    }
                    else
                    {
                        site.Job2Path.ElementAt(i).position = listJob2Positions.ElementAt(i);
                    }
                }
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void SetLevelNo(int levelNoLinkHigherJob1)
        {
            if (ModelState.IsValid)
            {
                var qry = from s in db.RecruitingSites
                          select s;
                qry = qry.Where(s => s.ID == defaultSiteID);
                RecruitingSite site = qry.FirstOrDefault();

                site.levelNoLinkHigherJob1 = levelNoLinkHigherJob1;
                db.SaveChanges();
            }
        }

        //ToDo: this method has not been finished yet
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
        public JsonResult GetJobs(int siteId)
        {

            var qry = from s in db.RecruitingSites
                      select s;
            qry = qry.Where(s => s.ID == siteId);
            RecruitingSite site = qry.FirstOrDefault();

            List<PathNode> jobsPath = new List<PathNode>();
            foreach (PathNode p in site.JobPath)
            {
                PathNode pn = new PathNode();
                pn.position = p.position;
                pn.hasCommonParent = p.hasCommonParent;
                jobsPath.Add(pn);
            }

            return Json(jobsPath, JsonRequestBehavior.AllowGet);

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
            iframe.ID = (int)id;

            return View(iframe);

        }

    }


}