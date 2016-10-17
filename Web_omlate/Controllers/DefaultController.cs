using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Web_omlate.DAL;
using Web_omlate.Models;

namespace Web_omlate.Controllers
{
    public class DefaultController : Controller
    {
        FYPDBContext _db;

        public DefaultController ()
        {
            _db = new FYPDBContext();
        }

        // GET: Default
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult SignUp ()
        {
            return View();
        }

        [HttpPost]
        public bool SendMail(string to, string Subject, string Mailbody, string Replyto)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(to);
                mail.From = new MailAddress("test@trigma.info");
                mail.Subject = Subject;
                mail.ReplyToList.Add(Replyto);

                // string Body = _objModelMail.Body;
                mail.Body = Mailbody;
               // mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";

                smtp.UseDefaultCredentials = false;
                mail.IsBodyHtml = true;
                smtp.Port = 587;

                smtp.Credentials = new System.Net.NetworkCredential
                    ("test@trigma.info", "Testing@123"); 

                smtp.EnableSsl = true;
                smtp.Send(mail);
                mail.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
