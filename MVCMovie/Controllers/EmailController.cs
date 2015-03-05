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

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            //Thread oThread = new Thread(new ThreadStart(oAlpha.Beta));

            var t = new Thread(() => emailProcessor.sendAllJobs(address, password));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

    }


    //This class has to be run in a separate thread because of using WebBrowser
    public class EmailProcessor : Form
    {
        private string sentDate = "C:/Users/Frank/Documents/Visual Studio 2013/Projects/MVCMovie/sentDate.txt";
        private string dateFormat = "MM/dd/yy";

        // This method that will be called when the thread is started
        public void sendAllJobs(string address, string password)
        {
            RecruitingSiteDBContext db = new RecruitingSiteDBContext();
            var qry = from s in db.RecruitingSites
                      select s;
            qry = qry.Where(s => s.ID == 1);
            RecruitingSite site = qry.FirstOrDefault();

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

            HtmlElement parent1 = document.GetElementsByTagName("html")[0];
            HtmlElement node1; 

            int i;
            HtmlElementCollection children;

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
            bool isToday = true;

            foreach (HtmlElement child in children)
            {
                node1 = child;
                try
                {
                    for (; i >= 0; i--)
                    {
                        node1 = node1.Children[JobPath.ElementAt<PathNode>(i).position];
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
                if (node1 != null)
                {
                    body += node1.InnerHtml + "\n";
                }
            }

            sendEmail(address, password, body);

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
}