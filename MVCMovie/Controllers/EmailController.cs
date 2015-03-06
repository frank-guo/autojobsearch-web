﻿using System;
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

        public EmailProcessor()
        {
            site = Getsite(1);
        }

        // This method that will be called when the thread is started
        public void sendAllJobs(string address, string password)
        {
            WebClient client = new WebClient();
            string html = client.DownloadString(site.url);
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.Url = new Uri("about:blank"); ;
            HtmlDocument document = browser.Document.OpenNew(true);
            document.Write(html);

            string body = "";

            List<PathNode> JobPath = new List<PathNode>();
            foreach (PathNode p in site.JobPath)
            {
                JobPath.Add(p);
            }

            List<Company> companyPath = new List<Company>();
            foreach (Company p in site.companyPath)
            {
                companyPath.Add(p);
            }

            List<Others> othersPath = getOthers();

            //Since use Element, have to start from <html> because document is a node but not element
            HtmlElement parent1 = document.GetElementsByTagName("html")[0];
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
            //bool isToday = true;

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


                    //node1 is currently the common ancestor of joba and company
                    if (node1 != null && isTodayJob(node1, levelNoCommonAnstr))
                    {
                        body += node1.InnerHtml + "\n\n\n\n";
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

            sendEmail(address, password, body);

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
            string others = getOtherInfo(commonAncestor, levelNoCommonAnstr);
            if (others.Contains("Today"))
            {
                return true;
            }
            return false;
        }

        //Given the common ancestor of job1, company and others, Get the other info of this job
        private string getOtherInfo(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<Others> othersPath = getOthers();
            int idxCommonAnstr = (othersPath.Count - 1) - levelNoCommonAnstr;   //To get the index of common anstr, (othersPath.Count -1) has to be used here
                                                                                //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[othersPath[i].position];
            }

            return node.InnerText;
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