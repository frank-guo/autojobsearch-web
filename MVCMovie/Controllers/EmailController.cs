using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.Timers;

namespace MVCMovie.Controllers
{
    public class EmailController : Controller
    {

        private const string sendingTime = "16:00";
        private const double interval = 60 * 1000; //one second

        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        // Start to send Email periodically
        public string TurnOnSend(string address, string password)
        {


            Timer checkForTime = new Timer(interval);
            checkForTime.Elapsed += new ElapsedEventHandler((sender, e) => checkForTime_Elapsed(sender, e, address, password));
            checkForTime.Enabled = true;

            return "Send email successfully";
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
            return false;
        }

        private void sendEmail(string address, string password){


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
            mm.Body = "TEST";
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);

                DateTime dateTime = DateTime.Now;
                string dateFormat = "MM/dd/yy";
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

        }
    }
}