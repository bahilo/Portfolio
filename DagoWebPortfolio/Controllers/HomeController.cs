using System.Linq;
using System.Data;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using DagoWebPortfolio.Models.DisplayViewModel;
using System.Net;
using SendGrid;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace DagoWebPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();
        private  DBDisplayModelContext dbD = new DBDisplayModelContext();

        public HomeController()
        {
            initDisplay();
        }

        public ActionResult Index(bool isContactSend = false)
        {            
            ViewBag.EmailConfirmation = isContactSend;
            ViewBag.Object = JsonConvert.SerializeObject(ViewBag.EmailConfirmation);
            return View();
        }

        public ActionResult _Welcome()
        {
            return View();
        }

        public ActionResult About()
        {
            
            return View();
        }

        private void initDisplay()
        {
            var display = dbD.Displays.Include("AboutView").ToList().LastOrDefault();

            if (display != null)
            {
                ViewBag.Display = display;
                /*ViewBag.AboutPictureUrl = display.AboutView.Path + display.AboutView.FileName;
                ViewBag.AboutMe = display.AboutView.Name;
                ViewBag.AboutHeadZone1 = display.AboutView.HeadZone1;
                ViewBag.AboutHeadZone2 = display.AboutView.HeadZone2;
                ViewBag.AboutHeadZone3 = display.AboutView.HeadZone3;

                ViewBag.AboutBodyZone1 = display.AboutView.BodyZone1;
                ViewBag.AboutBodyZone2 = display.AboutView.BodyZone2;
                ViewBag.AboutBodyZone3 = display.AboutView.BodyZone3;
                ViewBag.AboutBodyZone4 = display.AboutView.BodyZone4;
                ViewBag.AboutBodyZone5 = display.AboutView.BodyZone5;*/
            }

            display = dbD.Displays.Include("WelcomeView").Where(x => x.WelcomeView.FileName != null).ToList().LastOrDefault();

            if (display != null)
            {

                ViewBag.WelcomePictureUrl = display.WelcomeView.Path + display.WelcomeView.FileName;
                ViewBag.WelcomeMe = display.WelcomeView.Name;
                ViewBag.WelcomeZone1 = display.WelcomeView.Zone1;
                ViewBag.WelcomeZone2 = display.WelcomeView.Zone2;
                ViewBag.WelcomeZone3 = display.WelcomeView.Zone3;
            }

        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact([Bind(Include = "ID,Name,Company,Email,Phone,Comments")] ContactViewModel contactsViewModel)
        {
            ViewBag.EmailConfirmation = false;

            if (ModelState.IsValid)
            {
                // Create network credentials to access your SendGrid account
                var username = "azure_412716207eb27d53ea3852e66901d62c@azure.com";
                var pswd = "sY91OZ4x1V43nAd";

                var credentials = new NetworkCredential(username, pswd);
                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Create the email object first, then add the properties.
                var myMessage = new SendGridMessage();

                // Add the message properties.
                myMessage.From = new MailAddress(contactsViewModel.Email);

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                                            {
                                                @"joel.dago@yahoo.fr",
                                                @"eric.dago.225@gmail.com"
                                            };

                myMessage.AddTo(recipients);

                myMessage.Subject = contactsViewModel.Name + " - " + contactsViewModel.Company;

                //Add the HTML and Text bodies
                myMessage.Html = "<p>" + contactsViewModel.Comments + "</p>";
                myMessage.Html += "<p>Phone: "+ contactsViewModel.Phone + "</p>";
                //myMessage.Text = contactsViewModel.Comments;
                
                // Send the email, which returns an awaitable task.
                await transportWeb.DeliverAsync(myMessage);

                ViewBag.EmailConfirmation = true;
                ViewBag.Object = JsonConvert.SerializeObject(ViewBag.EmailConfirmation);

                db.Contacts.Add(contactsViewModel);
                db.SaveChanges();

                return RedirectToAction("Index", new { isContactSend = true });

                /*var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                //message.To.Add(new MailAddress("eric.dago.225@gmail.com"));  // replace with valid value 
                message.To.Add(new MailAddress("joel.dago@yahoo.fr"));  // replace with valid value 
                message.From = new MailAddress(contactsViewModel.Email);  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, contactsViewModel.Name, contactsViewModel.Email, contactsViewModel.Comments);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        //UserName = "eric.dago.225@gmail.com",  // replace with valid value
                        //Password = "!bahilo225"  // replace with valid value smtp-mail.outlook.com

                        UserName = "azure_412716207eb27d53ea3852e66901d62c@azure.com",  // replace with valid value
                        Password = "sY91OZ4x1V43nAd"  // replace with valid value smtp-mail.outlook.com

                    };
                    smtp.Credentials = credential;
                    //smtp.Host = "smtp.gmail.com";
                    smtp.Host = "smtp.sendgrid.net";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

                ViewBag.EmailConfirmation = true;
                    ViewBag.Object = JsonConvert.SerializeObject(ViewBag.EmailConfirmation);

                    db.SaveChanges();

                    return RedirectToAction("Index", new { isContactSend = true });

                    
                }*/
            }
            return View(contactsViewModel);
        }


     }

}