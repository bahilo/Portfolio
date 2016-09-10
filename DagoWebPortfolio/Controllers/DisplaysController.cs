using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models.DisplayViewModel;
using System.Threading;
using System.Globalization;

namespace DagoWebPortfolio.Controllers
{
    [Authorize]
    public class DisplaysController : Controller
    {
        private DBDisplayModelContext db = new DBDisplayModelContext();

        // GET: Displays
        public ActionResult Index(string target, string from, string action)
        {

            ViewBag.Target = target;
            ViewBag.From = from;
            ViewBag.Action = action;

            string countryName = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault();
            if (!string.IsNullOrEmpty(countryName))
                return View(countryName+"");
            else
                return RedirectToAction("Index");

            //var display = db.Displays.Include("AboutView").Include("WelcomeView").ToList();

            //return View();
        }

        // GET: Displays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayViewModel displayViewModel = db.Displays.Where( x=>x.ID == id ).Include("AboutView").Include("WelcomeView").DefaultIfEmpty().Single();
            if (displayViewModel == null)
            {
                return HttpNotFound();
            }
            return View(displayViewModel);
        }

        // GET: Displays/Create
        public ActionResult Create(string target, string from, string action)
        {

            ViewBag.Target = target;
            ViewBag.From = from;
            ViewBag.Action = action;
            //db.Displays.Include("AboutView").Include("WelcomeView")
            return View();
        }

        // POST: Displays/Create 
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,lang,AboutView,WelcomeView")] DisplayViewModel displayViewModel, HttpPostedFileBase fileAbout, HttpPostedFileBase fileWelcome )
        {
            if (ModelState.IsValid)
            {
                addPictureTable(displayViewModel, fileAbout, fileWelcome);
                db.Displays.Add(displayViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(displayViewModel);
        }

        private void addPictureTable(DisplayViewModel display, HttpPostedFileBase fileAbout, HttpPostedFileBase fileWelcome)
        {            

            if (fileAbout != null)
            {
                display.AboutView.Path = "/Content/Images/About/";
                display.AboutView.FileName = fileAbout.FileName;
                fileAbout.SaveAs(HttpContext.Server.MapPath(display.AboutView.Path + display.AboutView.FileName));
            }

            if (fileWelcome != null)
            {
                display.WelcomeView.Path = "/Content/Images/Welcome/";
                display.WelcomeView.FileName = fileWelcome.FileName;
                fileWelcome.SaveAs(HttpContext.Server.MapPath(display.WelcomeView.Path + display.WelcomeView.FileName));
            }            
            
        }


        // GET: Displays/Edit/5
        public ActionResult Edit(int? id, string target, string from, string action)
        {

            ViewBag.Target = target;
            ViewBag.From = from;
            ViewBag.Action = action;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayViewModel displayViewModel = db.Displays.Include("AboutView").Include("WelcomeView").Where(x => x.ID == id).DefaultIfEmpty().Single();
            if (displayViewModel == null)
            {
                return HttpNotFound();
            }
            return View(displayViewModel);
        }

        // POST: Displays/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
                                    [Bind(Include = "ID,lang,AboutView,WelcomeView")] DisplayViewModel displayViewModel
                                    , HttpPostedFileBase fileAbout
                                    , HttpPostedFileBase fileWelcome
                                    , string origineFileAbout
                                    , string origineFileWelcome
                                    , string aboutID
                                    , string welcomeID
                                 )
        {
            if (ModelState.IsValid)
            {

                addOrUpdateDisplay(displayViewModel, fileAbout, fileWelcome, origineFileAbout, origineFileWelcome, aboutID, welcomeID);

                db.Entry(displayViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(displayViewModel);
        }

        private void addOrUpdateDisplay(DisplayViewModel display, HttpPostedFileBase fileAbout
                                    , HttpPostedFileBase fileWelcome
                                    , string origineFileAbout
                                    , string origineFileWelcome
                                    , string aboutID
                                    , string welcomeID)
        {
            


            /***** Welcome Page Dispay ****/

            int idAbout = Int32.Parse(aboutID);
            var origineAboutDisplay = db.DisplayAbout.Where(x => x.ID == idAbout).Single();
           
            origineAboutDisplay.Name = display.AboutView.Name;
            origineAboutDisplay.HeadZone1 = display.AboutView.HeadZone1;
            origineAboutDisplay.HeadZone2 = display.AboutView.HeadZone2;
            origineAboutDisplay.HeadZone3 = display.AboutView.HeadZone3;

            origineAboutDisplay.BodyZone1 = display.AboutView.BodyZone1;
            origineAboutDisplay.BodyZone2 = display.AboutView.BodyZone2;
            origineAboutDisplay.BodyZone3 = display.AboutView.BodyZone3;
            origineAboutDisplay.BodyZone4 = display.AboutView.BodyZone4;
            origineAboutDisplay.BodyZone5 = display.AboutView.BodyZone5;

            display.AboutView = origineAboutDisplay;

            if (fileAbout != null)
            {
                display.AboutView.Path = "/Content/Images/About/";
                string[] savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + display.AboutView.Path));
                var origineFileWithPath = Server.MapPath("~" + display.AboutView.Path) + origineFileAbout;

                foreach (var f in savedFiles)
                {
                    if (origineFileWithPath.Equals(f))
                        System.IO.File.Delete(f);
                }
                display.AboutView.FileName = fileAbout.FileName;
                fileAbout.SaveAs(HttpContext.Server.MapPath(display.AboutView.Path + display.AboutView.FileName));
            }


            /***** Welcome Page Dispay ****/

            int idWelcome = Int32.Parse(welcomeID);
            var origineWelcometDisplay = db.DisplayWelcome.Where(x => x.ID == idWelcome).Single();

            origineWelcometDisplay.Name = display.WelcomeView.Name;
            origineWelcometDisplay.Zone1 = display.WelcomeView.Zone1;
            origineWelcometDisplay.Zone2 = display.WelcomeView.Zone2;
            origineWelcometDisplay.Zone3 = display.WelcomeView.Zone3;
            display.WelcomeView = origineWelcometDisplay;

            if (fileWelcome != null)
            {
                display.WelcomeView.Path = "/Content/Images/Welcome/";
                string[] savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + display.WelcomeView.Path));
                var origineFileWithPath = Server.MapPath("~" + display.WelcomeView.Path) + origineFileWelcome;

                foreach (var f in savedFiles)
                {
                    if (origineFileWithPath.Equals(f))
                        System.IO.File.Delete(f);
                }
                display.WelcomeView.FileName = fileWelcome.FileName;
                fileWelcome.SaveAs(HttpContext.Server.MapPath(display.WelcomeView.Path + display.WelcomeView.FileName));
            }
        }

        // GET: Displays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayViewModel displayViewModel = db.Displays.Find(id);
            if (displayViewModel == null)
            {
                return HttpNotFound();
            }
            return View(displayViewModel);
        }

        // POST: Displays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisplayViewModel displayViewModel = db.Displays.Find(id);
            db.Displays.Remove(displayViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
