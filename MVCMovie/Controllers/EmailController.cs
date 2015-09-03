using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Timers;
using MVCMovie.Models;
using System.ComponentModel;
using MVCMovie.Models.Enum;
using MVCMovie.Common;

using System.Windows.Forms;
using System.Threading;




namespace MVCMovie.Controllers
{
    //This class has to be run in a separate thread because of using WebBrowser
    //WebBrowser is used for getting the element in a html page by JobPath/companyPath etc. in RecruitingSite.
    public class EmailProcessor : Form
    {
        private const string sendingTime = "00:05";
        private const int onesecond = 1000;
        private const int twominTest = 120000;
        private const int oneday = 86400000;
        private const int twoday = 86400000 * 2;
        private const int oneweek = 86400000 * 7;
        private const int oneminute = 60000;
        private const int tensecond = 10000;

        private RecruitingSite site;
        private HtmlDocument document;
        private HtmlElement htmlElement;
        private  RecruitingSiteDBContext db = new RecruitingSiteDBContext();
        private int siteId;
        
        private System.Windows.Forms.Timer timer;
        private Email email;
        private WebBrowser browser;
        private WebClient client;
        private bool exitFlag;
        private string emailSubject;

        internal static Dictionary<int, Email> emails = new Dictionary<int, Email>();
        internal static Dictionary<int, System.Windows.Forms.Timer> timers = new Dictionary<int, System.Windows.Forms.Timer>();

        public EmailProcessor(Email eml)
        {
            timer = new System.Windows.Forms.Timer();
            SetTimer(eml, timer);
            timers.Add(eml.ID, timer);
            email = eml;
            emails.Add(eml.ID, eml);
            siteId = eml.ID;
            site = Getsite(siteId);
        }

        public void StartTimer()
        {
            while (!timeIsReady())
            {
                Thread.Sleep(oneminute);
            }

            exitFlag = false;

            browser = new WebBrowser();
            client = new WebClient();
            browser.ScriptErrorsSuppressed = true;

            //If don't set Url about:blank, browser.Document will be null, which causes a null reference exception
            //Since there is no actual form there.
            //ToDo: Subscribe to the webBrowser.DocumentCompleted event so document could be loaded.
            //Refer to http://stackoverflow.com/questions/9925022/webbrowser-document-is-always-null 
            browser.Url = new Uri("about:blank");

            sendAllJobs();
            timer.Tick += new EventHandler(OnTimedEvent);
            timer.Start();

            //This loop is needed or the timer event would not fire
            while (exitFlag == false && timers.ContainsKey(email.ID))
            {
                // Processes all the events in the queue.
                Application.DoEvents();
                Thread.Sleep(onesecond);
            }
        }

        internal static void SetEmail(Email email)
        {
            var eml = emails[email.ID];
            if(eml==null)
            {
                return;
            }

            eml.address = email.address;
            eml.password = email.password;
            eml.frequency = email.frequency;
            eml.sendingOn = email.sendingOn;
        } 

        private void OnTimedEvent(Object source, EventArgs e)
        {
            var retreivedEmail = GetEmail(siteId);

            if (retreivedEmail == null)
            {
                timer.Enabled = false;
                timers.Remove(siteId);
                emails.Remove(siteId);
                exitFlag = true;
            }
            else
            {
                if (retreivedEmail.sendingOn)
                {
                    //Re-set timer in case the frequency has been changed
                    SetTimer(retreivedEmail, timer);
                    email = retreivedEmail;
                    sendAllJobs();
                }
                else
                {
                    timer.Enabled = false;
                    timers.Remove(siteId);
                    emails.Remove(siteId);
                    exitFlag = true;
                }
            }

            
        }

        internal static void SetTimer(Email email, System.Windows.Forms.Timer timer)
        {
            switch (email.frequency)
            {
                case SendingFrequency.Daily:
                    timer.Interval = oneday;                    
                    break;
                case SendingFrequency.EveryOtherDay:
                    timer.Interval = twoday;
                    break;
                case SendingFrequency.Weekly:
                    timer.Interval = oneweek;
                    break;
            }
        }

        private Email GetEmail(int id)
        {
            var qry = from s in db.Emails
                      select s;
            qry = qry.Where(s => s.ID == email.ID);
            var retreivedEmail = qry.FirstOrDefault();

            return retreivedEmail;
        }


        // This method that will be called when the timer is out
        private void sendAllJobs()
        {
            var address = email.address;
            var password = email.password;
            string html;           
            string body = "";

            List<PathNode> JobPath = getJobPath();
            List<Company> companyPath = getCompanyPath();
            List<Others> othersPath = getOthers();

            if (JobPath.Count == 0 || companyPath.Count == 0 || othersPath.Count == 0)
            {
                body = MVCMovie.Resources.Email.SetConditionJobFeatures;
                goto sending;
            }
            
            //bool isToday = true;

            string url = site.url;

            //Loop through all the job posting pages until the posting time falls out of the specified range
            while (true)
            {

                html = client.DownloadString(url);
                document = browser.Document.OpenNew(true);

                document.Write(html);
                htmlElement = document.GetElementsByTagName("html")[0];

                //Since use Element, have to start from <html> because document is a node but not element
                HtmlElement parent1 = htmlElement;
                HtmlElement node1, job1, company;
                HtmlElementCollection children;

                //Caculate the common ancestor of Job1, company and others
                int levelNoCommonAnstr = 1;     //This refers to the level count from the child of html node
                //html node is actually level 0
                int i;
                int maxIdxJobPath = JobPath.Count - 1;
                int maxIdxCompPath = companyPath.Count - 1;

                //Go downwards from top until the common ancestor
                do
                {
                    //start from <html> element, i.e. level 1
                    children = parent1.Children;

                    job1 = children[JobPath[maxIdxJobPath - levelNoCommonAnstr].position];
                    company = children[companyPath[maxIdxCompPath - levelNoCommonAnstr].position];
                    parent1 = job1;
                    levelNoCommonAnstr++;
                } while (job1 == company);

                //At this point, levelNoCommonAnstr is 2 larger than the actual index of the common ancestor
                //since levelNoCommonAnstr++ at the end of the loop and  job1 already diffs from company before the last levelNoCommonAnstr++
                levelNoCommonAnstr -= 2;
                var idxCommonAnstr = maxIdxJobPath - levelNoCommonAnstr;



                //Get all the job nodes
                //First get to the commmon Parent of job1 and job2
                parent1 = document.GetElementsByTagName("html")[0];
                for (i = JobPath.Count - 2; i >= 0; i--)
                {
                    if (!(JobPath.ElementAt<PathNode>(i).hasCommonParent))
                    {
                        //Get the child of parent1 at the position
                        children = parent1.Children;
                        node1 = children[JobPath.ElementAt<PathNode>(i).position];

                        parent1 = node1;
                    }
                    else
                    {
                        break;
                    }
                }

                //parent1 is currently at th level of the common ancestor
                //i is currently at the level one lower than the common ancestor
                //Get all the job title nodes
                var startIdx = --i;
                children = parent1.Children;

                //Iterate all the immediate children of the common ancestor of job1 and job2  to get all the jobs
                foreach (HtmlElement child in children)
                {
                    node1 = child;
                    try
                    {
                        for (; i >= idxCommonAnstr; i--)
                        {
                            node1 = node1.Children[JobPath.ElementAt<PathNode>(i).position];
                        }

                        if (node1 != null)
                        {
                            //Tell if the job is in the specified time range
                            if (!isJobInTimeRange(node1, levelNoCommonAnstr))
                            {
                                goto sending;
                            }

                            //node1 is currently the common ancestor of joba and company
                            bool isContainsTitle = false;
                            bool isContainsLocation = false;

                            var conds = from s in db.Conditions
                                            select s;
                            conds = conds.Where(s => s.ID == siteId);
                            Condition cond = conds.FirstOrDefault();

                            //Evaluate title conditions
                            foreach (var titleCond in cond.titleConds)
                            {
                                if (getJobTitleNode(node1, levelNoCommonAnstr).InnerText.Contains(titleCond.titleCond))
                                {
                                    isContainsTitle = true;
                                    break;
                                }
                            }

                            //Evaluate location conditions
                            foreach (var locationCond in cond.locationConds)
                            {
                                if ( getOtherInfo(node1, levelNoCommonAnstr).InnerText.Contains(locationCond.locationCond) ) {
                                    isContainsLocation = true;
                                    break;
                                }
                            }

                            //If this job satisfies the both types of conditions, then concatenate it to the email body
                            if (isContainsTitle && isContainsLocation)
                            {
                                body += getJobTitleNode(node1, levelNoCommonAnstr).InnerText + " " + getCompanyNameNode(node1, levelNoCommonAnstr).InnerText
                                    + " " + getOtherInfo(node1, levelNoCommonAnstr).InnerText
                                    + "\n" + getBaseUrl(site.url);

                                //Append jobs' links to the email body
                                HtmlElement jobLink = getJobTitleNode(node1, levelNoCommonAnstr);
                                //Iterate to the parent link node of this job
                                for (int l = 0; l < site.levelNoLinkHigherJob1; l++)
                                {
                                    jobLink = jobLink.Parent;
                                }
                                body += jobLink.GetAttribute("href").Substring(6) + "\n\n\n";
                            }
                        }
                    }
                    //Some of branches of the common ancestor of Node1 and Node2 might not be the job title,
                    //e.g. The leaf of the first branch on T-Net is a img tag.
                    //Thus, this case may throw out an out-of-bound excpetion
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    //check if it is still today's job
                    i = startIdx;
                }

                url = getNextUrl().Substring(6);    //delete "about:"

                if (!url.StartsWith("http"))
                {
                    string preStr = getBaseUrl(site.url);
                    url = preStr + url;

                }

                if ( url == null)
                {
                    break;
                }

            }

            sending:

            html = client.DownloadString(site.url);
            document = browser.Document.OpenNew(true);
            document.Write(html);
            var titles = document.GetElementsByTagName("title");
            emailSubject = titles == null ? "" : titles[0].InnerHtml;

            sendEmail(address, password, body);

        }

        private bool timeIsReady()
        {
            DateTime dateTime = DateTime.Now;
            string timeFormat = "HH:mm";

            string currentTime = dateTime.ToString(timeFormat);

            if (currentTime.CompareTo(sendingTime) > 0)
            {

                return true;
            }

            return false;

        }

        private string getBaseUrl(string url)
        {
            string baseUrl = null;

            if (url != null && url.StartsWith("http"))
            {
                int count = 0;
                int j = 0;

                for (; j <= site.url.Length - 1 && count < 3; j++)
                {

                    if (site.url.ElementAt<char>(j) == '/')
                    {
                        count++;
                    }
                }

                //At this point, j points to the char right after the third '/'
                //j-1 represents the length of the substring ahead of the third '/'
                baseUrl = site.url.Substring(0, j - 1);
            }

            return baseUrl;
        }


        private List<PathNode> getJobPath()
        {
            List<PathNode> JobPath = new List<PathNode>();

            foreach (PathNode p in site.JobPath)
            {
                JobPath.Add(p);
            }

            return JobPath;
        }

        private List<Company> getCompanyPath()
        {
            List<Company> companyPath = new List<Company>();

            foreach (Company p in site.companyPath)
            {
                companyPath.Add(p);
            }

            return companyPath;
        }

        //Given the common ancestor of job1, company and others, Get the Node of this job title
        private HtmlElement getJobTitleNode(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<PathNode> jobPath = getJobPath();

            if (jobPath.Count == 0)
            {
                return null;
            }
            
            int idxCommonAnstr = (jobPath.Count - 1) - levelNoCommonAnstr;  //To get the index of common anstr, (jobPath.Count -1) has to be used here
                                                                            //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[jobPath[i].position];
            }

            return node;
        }

        //Given the common ancestor of job1, company and others, Get the Node of this job company
        private HtmlElement getCompanyNameNode(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<Company> companyPath = getCompanyPath();

            if (companyPath.Count == 0)
            {
                return null;
            }

            int idxCommonAnstr = (companyPath.Count - 1) - levelNoCommonAnstr;  //To get the index of common anstr, (companyPath.Count -1) has to be used here
                                                                                //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[companyPath[i].position];
            }

            return node;
        }

        private string getNextUrl()
        {
            List<NextPosition> nextPath = getNext();
            HtmlElement parent = htmlElement;
            HtmlElement node = null;

            for (int i = nextPath.Count - 2; i >= 0; i--)
            {
                node = parent.Children[nextPath[i].position];
                parent = node;
            }

            if ( node != null )
            {
                return node.GetAttribute("href");
            }

            return null;
        }

        private List<NextPosition> getNext()
        {
            List<NextPosition> nextPath = new List<NextPosition>();
            foreach (NextPosition p in site.ListNextPositions)
            {
                nextPath.Add(p);
            }

            return nextPath;
        }

        private List<Others> getOthers()
        {
            List<Others> othersPath = new List<Others>();
            foreach (Others p in site.othersPath)
            {
                othersPath.Add(p);
            }

            return othersPath;
        }

        private RecruitingSite Getsite(int id){
            RecruitingSiteDBContext db = new RecruitingSiteDBContext();

            var qry = from s in db.RecruitingSites
                      select s;
            qry = qry.Where(s => s.ID == id);
            RecruitingSite site = qry.FirstOrDefault();

            return site;
        }

        //Given the common ancestor of job1, company and others, tell if the job is for today
        private bool isJobInTimeRange(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            HtmlElement others = getOtherInfo(commonAncestor, levelNoCommonAnstr);

            switch (email.frequency)
            {
                case SendingFrequency.Daily:
                    if (others.InnerText.Contains("Today"))
                    {
                        return true;
                    }
                    break;
                case SendingFrequency.EveryOtherDay:
                    if (others.InnerText.Contains("Today") || others.InnerText.Contains("Yesterday"))
                    {
                        return true;
                    }
                    break;
                case SendingFrequency.Weekly:
                    //ToDo: Write a function to tell if the date is in the recent seven days
                    if (others.InnerText.Contains("Today") || others.InnerText.Contains("Yesterday"))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        //Given the common ancestor of job1, company and others, Get the other info of this job
        private HtmlElement getOtherInfo(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<Others> othersPath = getOthers();

            if (othersPath.Count == 0)
            {
                return null;
            }

            int idxCommonAnstr = (othersPath.Count - 1) - levelNoCommonAnstr;   //To get the index of common anstr, (othersPath.Count -1) has to be used here
                                                                                //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[othersPath[i].position];
            }

            return node;
        }

        private void sendEmail(string address, string password, string body)
        {

            SmtpClient client = new SmtpClient();

            client.Port = email.smtpPort;
            client.Host = email.smtpAddress;
            client.EnableSsl = true;
            client.Timeout = tensecond;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(address, password);

            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(address);
                mm.To.Add(new MailAddress(address));
                mm.Subject = emailSubject;
                mm.Body = body;

                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
            }
            catch (Exception ex)
            {
                var logger = Logger.GetInstance();
                logger.WriteLine(DateTime.Now.ToString());
                logger.WriteLine(String.Format(ex.ToString()));
            }
        }

    }


    public class EmailController : Controller
    {
        private RecruitingSiteDBContext db = new RecruitingSiteDBContext();

        // GET: Email
        public ActionResult Index(int id)
        {
            var values = Enum.GetValues(typeof(SendingFrequency));
            int i = 0;
            string[] descriptions = new string[values.Length];

            foreach (SendingFrequency value in values)
            {
                descriptions[i] = value.ToName();
                i++;
             }
            ViewBag.SendingFrequencies = descriptions;
            ViewBag.siteId = id;

            return View();
        }

        public ActionResult SaveEmail(Email email)
        {
            if (email != null && email.ID <= 0)
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

                var qry = from s in db.Emails
                          select s;
                qry = qry.Where(s => s.ID == email.ID);
                var retreivedEmail = qry.FirstOrDefault();

                //The email of email ID doesn't exist, and then insert the new email
                //Othewise, remove the original one and insert the new email
                if (retreivedEmail != null)
                {
                    db.Emails.Remove(retreivedEmail);
                    db.SaveChanges();           
                }
                
                db.Emails.Add(email);
                db.SaveChanges();

                if (email.sendingOn)
                {
                    TurnOnSend(email);
                }
                else
                {
                    EmailProcessor.timers[email.ID].Enabled = false;
                    EmailProcessor.timers.Remove(email.ID);
                    EmailProcessor.emails.Remove(email.ID);
                }

                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.Success,
                    message = String.Format(Resources.Common.SaveSuccessMsg, Resources.Email.email),
                };
                return Json(response);
            }
            else
            {
                var response = new Common.Model.Message()
                {
                    msgCode = Common.Model.MessageCode.InvlidModel,
                    message = String.Format(Resources.Common.InvlidModel, Resources.Email.email),
                };
                return Json(response);
            }
        }

        public JsonResult GetEmail(int id)
        {

            var qry = from s in db.Emails
                      select s;
            qry = qry.Where(s => s.ID == id);
            Email email = qry.FirstOrDefault();

            if (email == null)
            {
                return null;
            }

            //Don't return RecruitingSite property or a cyclic reference error occurs.
            return Json(new {
                address = email.address,
                password = email.password,
                frequency = email.frequency,
                sendingOn = email.sendingOn,
                smtpAddress = email.smtpAddress,
                smtpPort = email.smtpPort,
            }, 
            JsonRequestBehavior.AllowGet);

        }

        // Start to send Email periodically
        private void TurnOnSend(Email email)
        {
            sendEmail(email);

            return;
        }

        private void sendEmail(Email email)
        {
            //If there is already a thread to send email for this site, don't start a new thread
            if (!EmailProcessor.timers.ContainsKey(email.ID)) 
            {
                EmailProcessor emailProcessor = new EmailProcessor(email);

                // Create the thread object, passing in the EmailProcessor.sendAllJobs method
                // via a ThreadStart delegate. This does not start the thread.

                var t = new Thread(() => emailProcessor.StartTimer());

                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            else
            {
                EmailProcessor.SetEmail(email);
                var timer = EmailProcessor.timers[email.ID];
                EmailProcessor.SetTimer(email, timer);
            }
        }

    }
}