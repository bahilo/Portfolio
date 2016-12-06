using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using QCBDManagementCommon.Classes;
using System.Data.Entity.Validation;

namespace DagoWebPortfolio.Controllers
{
    public class TechnoEnvironmentsController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: TechnoEnvironments
        public ActionResult Index()
        {
            var technoEnv = db.TechnoEnv.Include(t => t.Education).Include(t => t.Experience).Include(t => t.Picture).Include(t => t.Project).Include(t => t.ProjectDetail).Include(t => t.Skill);
            return View(technoEnv.ToList());
        }

        // GET: TechnoEnvironments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnoEnvironmentsViewModel technoEnvironmentsViewModel = db.TechnoEnv.Find(id);
            if (technoEnvironmentsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(technoEnvironmentsViewModel);
        }

        // GET: TechnoEnvironments/Create
        public ActionResult Create()
        {
            setSourceDropDownList(new TechnoEnvironmentsViewModel());
            return View();
        }

        // POST: TechnoEnvironments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,Category,path,FileName,link,ProjectsViewModelID,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID,PicturesViewModelID")] TechnoEnvironmentsViewModel technoEnvironmentsViewModel, string selectedCategory
            , bool link_TechEnv_to_picture
            , bool link_TechEnv_to_project
            , bool link_TechEnv_to_project_detail
            , bool link_TechEnv_to_experience
            , bool link_TechEnv_to_education
            , bool link_TechEnv_to_skill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    addOrUpdateDisplayTable(technoEnvironmentsViewModel, selectedCategory
                        , link_TechEnv_to_picture
                        , link_TechEnv_to_project
                        , link_TechEnv_to_project_detail
                        , link_TechEnv_to_experience
                        , link_TechEnv_to_education
                        , link_TechEnv_to_skill
                        , false);
                    db.TechnoEnv.Add(technoEnvironmentsViewModel);
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
            setSourceDropDownList(technoEnvironmentsViewModel);
            return View(technoEnvironmentsViewModel);
        }

        // GET: TechnoEnvironments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnoEnvironmentsViewModel technoEnvironmentsViewModel = db.TechnoEnv.Find(id);
            if (technoEnvironmentsViewModel == null)
            {
                return HttpNotFound();
            }
            setSourceDropDownList(technoEnvironmentsViewModel);
            return View(technoEnvironmentsViewModel);
        }

        // POST: TechnoEnvironments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,Category,path,FileName,link,ProjectsViewModelID,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID,PicturesViewModelID")] TechnoEnvironmentsViewModel technoEnvironmentsViewModel, string selectedCategory
            , bool link_TechEnv_to_picture
            , bool link_TechEnv_to_project
            , bool link_TechEnv_to_project_detail
            , bool link_TechEnv_to_experience
            , bool link_TechEnv_to_education
            , bool link_TechEnv_to_skill
            , bool link_TechEnv_to_none)
        {
            if (ModelState.IsValid)
            {
                addOrUpdateDisplayTable(technoEnvironmentsViewModel, selectedCategory
                        , link_TechEnv_to_picture
                        , link_TechEnv_to_project
                        , link_TechEnv_to_project_detail
                        , link_TechEnv_to_experience
                        , link_TechEnv_to_education
                        , link_TechEnv_to_skill
                        , link_TechEnv_to_none);
                db.Entry(technoEnvironmentsViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            setSourceDropDownList(technoEnvironmentsViewModel);
            return View(technoEnvironmentsViewModel);
        }

        // GET: TechnoEnvironments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnoEnvironmentsViewModel techEnv = db.TechnoEnv.Find(id);   
            if (techEnv == null)
            {
                return HttpNotFound();
            }
            return View(techEnv);
        }

        // POST: TechnoEnvironments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TechnoEnvironmentsViewModel techEnv = db.TechnoEnv.Find(id);
            techEnv.Education = null;
            techEnv.EducationViewModelID = null;
            techEnv.ProjectDetail = null;
            techEnv.ProjectDetailsViewModelID = null;
            techEnv.Experience = null;
            techEnv.ExperiencesViewModelID = null;
            techEnv.Skill = null;
            techEnv.SkillsViewModelID = null;
            techEnv.Project = null;
            techEnv.ProjectsViewModelID = null;            
            try
            {
                db.TechnoEnv.Remove(techEnv);
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

        private void setSourceDropDownList(TechnoEnvironmentsViewModel techEnv)
        {
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", techEnv.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", techEnv.ExperiencesViewModelID);
            ViewBag.ProjectsViewModelID = new SelectList(db.Projects, "ID", "Title", techEnv.ProjectsViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", techEnv.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", techEnv.SkillsViewModelID);
            ViewBag.PicturesViewModelID = new SelectList(db.PicturesApp, "ID", "Subject", techEnv.PicturesViewModelID);
            ViewBag.CategoriesID = new SelectList(new List<string> { "Language", "Technology", "Version Control", "Web Service" }, techEnv.Category);
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

        private void addOrUpdateDisplayTable(TechnoEnvironmentsViewModel techEnv, string selectedCategory
            , bool link_TechEnv_to_picture
            , bool link_TechEnv_to_project
            , bool link_TechEnv_to_project_detail
            , bool link_TechEnv_to_experience
            , bool link_TechEnv_to_education
            , bool link_TechEnv_to_skill
            , bool link_TechEnv_to_none)
        {           

            if (link_TechEnv_to_education)
                techEnv.Education = db.Education.Find(techEnv.EducationViewModelID);//db.Education.Find(display.EducationViewModelID);
            else
                techEnv.EducationViewModelID = null;

            if (link_TechEnv_to_experience)
                techEnv.Experience = db.Experiences.Find(techEnv.ExperiencesViewModelID);//db.Education.Find(display.EducationViewModelID);
            else
                techEnv.ExperiencesViewModelID = null;

            if (link_TechEnv_to_picture)
                techEnv.Picture = db.PicturesApp.Find(techEnv.PicturesViewModelID);//db.Education.Find(display.EducationViewModelID);
            else
                techEnv.PicturesViewModelID = null;

            if (link_TechEnv_to_project)
                techEnv.Project = db.Projects.Find(techEnv.ProjectsViewModelID);//db.Education.Find(display.EducationViewModelID);
            else
                techEnv.ProjectsViewModelID = null;

            if (link_TechEnv_to_project_detail)
                techEnv.ProjectDetail = db.DetailsProject.Find(techEnv.ProjectDetailsViewModelID);//db.Education.Find(display.EducationViewModelID);
            else
                techEnv.ProjectDetailsViewModelID = null;

            if (link_TechEnv_to_skill)
                techEnv.Skill = db.Skills.Find(techEnv.SkillsViewModelID);//db.Education.Find(display.EducationViewModelID);
            else
                techEnv.SkillsViewModelID = null;

            if (link_TechEnv_to_none)
            {
                techEnv.EducationViewModelID = null;
                techEnv.Education = null;
                techEnv.ProjectsViewModelID = null;
                techEnv.Project = null;
                techEnv.ExperiencesViewModelID = null;
                techEnv.Experience = null;
                techEnv.ProjectDetailsViewModelID = null;
                techEnv.ProjectDetail = null;
                techEnv.PicturesViewModelID = null;
                techEnv.Picture = null;
                techEnv.SkillsViewModelID = null;
                techEnv.Skill = null;
            }
            else
                techEnv.Category = selectedCategory;

        }
    }
}
