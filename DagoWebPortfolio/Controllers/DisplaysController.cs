using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using System.Globalization;
using DagoWebPortfolio.Classes;
using System.Data.Entity.Validation;
using QCBDManagementCommon.Classes;

namespace DagoWebPortfolio.Controllers
{
    public class DisplaysController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        public DisplaysController()
        {
            ViewBag.Cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures).ToList();
        }

        // GET: Displays
        public ActionResult Index()
        {
            return View(db.Displays.Include("Picture").Include("Education").Include("Experience").Include("ProjectDetail").Include("Skill").ToList());
        }

        // GET: Displays/Details/5
        public ActionResult Details(int? id)
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

        // GET: Displays/Create
        public ActionResult Create()
        {
            setSourceDropDownList(new DisplayViewModel());

            return View();
        }

        // POST: Displays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,Lang,Description,ProjectsViewModelID,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID,PicturesViewModelID")] DisplayViewModel displayViewModel, string selectedLang, string link_display_to)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var display = displayViewModel;
                    addOrUpdateDisplayTable(display, selectedLang, link_display_to);
                    db.Entry(display).State = EntityState.Added;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    ViewBag.ErrorMessage = retrievePropertiesErrors(dbEx);
                    return View("Error");
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message);
                    return View("Error");
                }
                return RedirectToAction("Index");
            }
            setSourceDropDownList(displayViewModel);
            return View(displayViewModel);
        }

        // GET: Displays/Edit/5
        public ActionResult Edit(int? id)
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
            setSourceDropDownList(displayViewModel);
            return View(displayViewModel);
        }

        // POST: Displays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,Lang,Description,ProjectsViewModelID,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID,PicturesViewModelID")] DisplayViewModel displayViewModel, string selectedLang, string link_display_to)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var display = displayViewModel;
                    addOrUpdateDisplayTable(display, selectedLang, link_display_to);
                    db.Entry(display).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    ViewBag.ErrorMessage = retrievePropertiesErrors(dbEx);
                    return View("Error");
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message);
                    return View("Error");
                }
                return RedirectToAction("Index");
            }
            return View(displayViewModel);
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
            DisplayViewModel display = db.Displays.Find(id);
            display.Education = null;
            display.EducationViewModelID = null;
            display.ProjectDetail = null;
            display.ProjectDetailsViewModelID = null;
            display.Experience = null;
            display.ExperiencesViewModelID = null;
            display.Skill = null;
            display.SkillsViewModelID = null;
            display.Project = null;
            display.ProjectsViewModelID = null;

            try
            {
                db.Displays.Remove(display);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
                return View("Error");
            }

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


        //===============================

        private void setSourceDropDownList(DisplayViewModel display)
        {
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", display.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", display.ExperiencesViewModelID);
            ViewBag.ProjectsViewModelID = new SelectList(db.Projects, "ID", "Title", display.ProjectsViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", display.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", display.SkillsViewModelID);
            ViewBag.PicturesViewModelID = new SelectList(db.PicturesApp, "ID", "Subject", display.PicturesViewModelID);
        }

        private string retrievePropertiesErrors(DbEntityValidationException dbEx)
        {
            string message = "";
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    message += string.Format("Property: {0} Error: {1} {2}",
                                            validationError.PropertyName,
                                            validationError.ErrorMessage,
                                            Environment.NewLine);
                }
            }            
            Log.error(message);
            return message;
        }

        private void addOrUpdateDisplayTable(DisplayViewModel display, string selectedLang, string link_display_to)
        {
            display.Lang = selectedLang;

            switch (link_display_to)
            {
                case "project":
                    display.Project = db.Projects.Find(display.ProjectsViewModelID);// (display.ProjectDetailsViewModelID);
                    display.EducationViewModelID = null;
                    display.ExperiencesViewModelID = null;
                    display.ProjectDetailsViewModelID = null;
                    display.SkillsViewModelID = null;
                    display.PicturesViewModelID = null;
                    break;
                case "project-detail":
                    display.ProjectDetail = db.DetailsProject.Find(display.ProjectDetailsViewModelID);// (display.ProjectDetailsViewModelID);
                    display.EducationViewModelID = null;
                    display.ProjectsViewModelID = null;
                    display.ExperiencesViewModelID = null;
                    display.SkillsViewModelID = null;
                    display.PicturesViewModelID = null;
                    break;
                case "education":
                    display.Education = db.Education.Find(display.EducationViewModelID);//db.Education.Find(display.EducationViewModelID);
                    display.ProjectDetailsViewModelID = null;
                    display.ExperiencesViewModelID = null;
                    display.SkillsViewModelID = null;
                    display.PicturesViewModelID = null;
                    display.ProjectsViewModelID = null;
                    break;
                case "experience":
                    display.Experience = db.Experiences.Find(display.ExperiencesViewModelID);
                    display.ProjectDetailsViewModelID = null;
                    display.EducationViewModelID = null;
                    display.SkillsViewModelID = null;
                    display.PicturesViewModelID = null;
                    display.ProjectsViewModelID = null;
                    break;
                case "skill":
                    display.Skill = db.Skills.Find(display.SkillsViewModelID);
                    display.ProjectDetailsViewModelID = null;
                    display.EducationViewModelID = null;
                    display.ExperiencesViewModelID = null;
                    display.PicturesViewModelID = null;
                    display.ProjectsViewModelID = null;
                    break;
                case "picture":
                    display.Picture = db.PicturesApp.Find(display.PicturesViewModelID);
                    display.ProjectDetailsViewModelID = null;
                    display.EducationViewModelID = null;
                    display.SkillsViewModelID = null;
                    display.ExperiencesViewModelID = null;
                    display.ProjectsViewModelID = null;
                    break;
                default:
                    display.PicturesViewModelID = null;
                    display.ProjectDetailsViewModelID = null;
                    display.EducationViewModelID = null;
                    display.SkillsViewModelID = null;
                    display.ExperiencesViewModelID = null;
                    display.ProjectsViewModelID = null;
                    break;
            }
        }




    }
}
