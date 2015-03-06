﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using MVCMovie.Models;

namespace MVCMovie.Controllers
{
    public class BrowserController : Controller
    {
        private RecruitingSiteDBContext db = new RecruitingSiteDBContext();

        [HttpPost]
        public void SetURL(string url)
        {
            if (url == null || url == "")
            {
                return;
            }

            RecruitingSite site = new RecruitingSite();
            site.url = url;

            if (ModelState.IsValid)
            {
                var siteUrls = new List<string>();
                var qry = from s in db.RecruitingSites
                          select s.url;
                qry = qry.Where(u => u == url);
                siteUrls.AddRange(qry.Distinct());
                if (siteUrls.Count == 0)    //check if the t-net url exists already
                {
                    db.RecruitingSites.Add(site);
                    db.SaveChanges();
                }
            }
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

        public JsonResult GeCompany()
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

        public string Index()
        {
            
            string webaddress = null;
            WebClient client = new WebClient();

            string webpage = null;

            //get the first site url
            var siteUrls = new List<string>();
            var qry = from s in db.RecruitingSites
                       select s.url;
            siteUrls.AddRange(qry.Distinct());

            //ToDo: To make things simple, the first row of query result is always used to open the browser window.
            //ToDo: Make it use the selected website to open the browser window in the future.
            webaddress = siteUrls[0];

            if (webaddress != null)
            {
                try
                {
                    webpage = client.DownloadString(webaddress);
                }
                catch { 
                    
                }
            }
            else
            {
                return null;
            }
            string webpageUpdate = webpage;

            if (webpage.Contains("/js/jquery.js"))
            {

                webpageUpdate = webpage.Replace("/js/jquery.js", "http://www.bctechnology.com/js/jquery.js");

                webpageUpdate = webpageUpdate.Replace("/js/ui.core.js", "http://www.bctechnology.com/js/ui.core.js");

                webpageUpdate = webpageUpdate.Replace("/js/ui.dropdownchecklist.js", "http://www.bctechnology.com/js/ui.dropdownchecklist.js");

                webpageUpdate = webpageUpdate.Replace("/js/anylinkmenu.js", "http://www.bctechnology.com/js/anylinkmenu.js");

                webpageUpdate = webpageUpdate.Replace("/styles/tnet.css", "http://www.bctechnology.com/styles/tnet.css");

                webpageUpdate = webpageUpdate.Replace("/styles/tnet-footer.css", "http://www.bctechnology.com/styles/tnet-footer.css");

                webpageUpdate = webpageUpdate.Replace("/styles/ui.dropdownchecklist.css", "http://www.bctechnology.com/styles/ui.dropdownchecklist.css");

                webpageUpdate = webpageUpdate.Replace("/styles/anylinkmenu.css", "http://www.bctechnology.com/styles/anylinkmenu.css");

                webpageUpdate = webpageUpdate.Replace("/js/tnet.utils.js", "http://www.bctechnology.com/js/tnet.utils.js");
            }

            return webpageUpdate;

        }
    }
}