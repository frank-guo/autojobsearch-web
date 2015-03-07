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
//using HtmlAgilityPack;
using System.Windows.Forms;
using System.Threading;



namespace MVCMovie.Controllers
{

    //This class has to be run in a separate thread because of using WebBrowser
    public class EmailProcessor : Form
    {
        private string sentDate = "C:/Users/Frank/Documents/Visual Studio 2013/Projects/MVCMovie/sentDate.txt";
        private string dateFormat = "MM/dd/yy";
        private RecruitingSite site;
        private HtmlDocument document;
        private HtmlElement htmlElement;

        public EmailProcessor()
        {
            site = Getsite(1);
        }

        // This method that will be called when the thread is started
        public void sendAllJobs(string address, string password)
        {
            WebBrowser browser = new WebBrowser();
            WebClient client = new WebClient();
            browser.ScriptErrorsSuppressed = true;

            //If don't set Url about:blank, browser.Document will be null, which causes a null reference exception
            //Since there is no actual form there.
            //ToDo: Subscribe to the webBrowser.DocumentCompleted event so document could be loaded.
            //Refer to http://stackoverflow.com/questions/9925022/webbrowser-document-is-always-null
            browser.Url = new Uri("about:blank");
            

            string html;
            
            string body = "";

            List<PathNode> JobPath = getJobPath();

            List<Company> companyPath = getCompanyPath();

            List<Others> othersPath = getOthers();
            
            //bool isToday = true;

            string url = site.url;

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
                            if (!isTodayJob(node1, levelNoCommonAnstr))
                            {
                                goto sending;
                            }

                            //node1 is currently the common ancestor of joba and company
                            body += getJobTitleNode(node1, levelNoCommonAnstr).InnerText + " " + getCompanyNameNode(node1, levelNoCommonAnstr).InnerText
                                + " " + getOtherInfo(node1, levelNoCommonAnstr).InnerText + "\n\n\n";
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
                    string preStr = site.url.Substring(0, j - 1);
                    url = preStr + url;

                }

                if ( url == null)
                {
                    break;
                }

            }

            sending:
            sendEmail(address, password, body);

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
        private bool isTodayJob(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            HtmlElement others = getOtherInfo(commonAncestor, levelNoCommonAnstr);

            if (others.InnerText.Contains("Today"))
            {
                return true;
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
            client.Port = 587;
            client.Host = "mailgate.sfu.ca";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(address, password);

            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(address);
            mm.To.Add(new MailAddress(address));
            mm.Subject = "T-Net Jobs - Today";
            mm.Body = body;

            //mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);

                DateTime dateTime = DateTime.Now;
                string today = dateTime.ToString(dateFormat);
                setHasSent(today);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
            }
        }

        private void setHasSent(string today)
        {
            using (StreamWriter file = new StreamWriter(sentDate))
            {
                try
                {
                    file.Write(today);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

    }


    public class EmailController : Controller
    {

        private const string sendingTime = "00:05";
        private const double interval = 60 * 1000; //one second
        private string sentDate = "C:/Users/Frank/Documents/Visual Studio 2013/Projects/MVCMovie/sentDate.txt";
        private string dateFormat = "MM/dd/yy";

        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        // Start to send Email periodically
        public string TurnOnSend(string address, string password)
        {

            //Timer checkForTime = new Timer(interval);
            //checkForTime.Elapsed += new ElapsedEventHandler((sender, e) => 
            //    checkForTime_Elapsed(sender, e, address, password));
            // checkForTime.Enabled = true;

            sendEmail(address, password);

            return "Turn on email-sending successfully";
        }

        private void checkForTime_Elapsed(object sender, ElapsedEventArgs e, string address, string password)
        {
            if (timeIsReady() && !hasSent())
            {
                sendEmail(address, password);
            }
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

        private bool hasSent()
        {

            if (!System.IO.File.Exists(sentDate))
            {
                return false;
            }

            DateTime dateTime = DateTime.Now;
            string today = dateTime.ToString(dateFormat);

            using (StreamReader file = new StreamReader(sentDate))
            {
                try
                {
                    string date = file.ReadLine();
                    if (date.Equals(today))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return false;
        }

        private void sendEmail(string address, string password)
        {

            EmailProcessor emailProcessor = new EmailProcessor();

            // Create the thread object, passing in the EmailProcessor.sendAllJobs method
            // via a ThreadStart delegate. This does not start the thread.

            var t = new Thread(() => emailProcessor.sendAllJobs(address, password));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

    }



}