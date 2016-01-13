using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;

namespace DagoWebPortfolio2.Controllers
{
    public class PicturesOld2Controller : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Pictures
        public ActionResult Index()
        {
            var picturesProject = db.PicturesApp.Include(p => p.Education).Include(p => p.Experience).Include(p => p.ProjectDetail).Include(p => p.Skill);
            return View(picturesProject.ToList());
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PicturesViewModel projectPicturesViewModel = db.PicturesApp.Find(id);
            if (projectPicturesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectPicturesViewModel);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName");
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title");
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject");
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title");
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,path,FileName,Description,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID")] PicturesViewModel projectPicturesViewModel, HttpPostedFileBase file, string img_subject_link)
        {
            /*if (ModelState.IsValid)
            {
                db.PicturesProject.Add(projectPicturesViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }*/

            if (ModelState.IsValid && file != null)
            {
                var picture = projectPicturesViewModel;
                addOrUpdatePictureTable(picture,file, img_subject_link);
                db.PicturesApp.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", projectPicturesViewModel.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", projectPicturesViewModel.ExperiencesViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", projectPicturesViewModel.SkillsViewModelID);
            return View(projectPicturesViewModel);
        }

        private void addOrUpdatePictureTable(PicturesViewModel picture, HttpPostedFileBase file, string img_subject_link)
        {
            /*var localPicture = new ProjectPicturesViewModel
            {
                ID = picture.ID,
                Description = picture.Description,
                Subject = picture.Subject

            };*/
            switch (img_subject_link)
            {
                case "project":
                    var projectDetail = db.DetailsProject.Find(picture.ProjectDetailsViewModelID);
                    picture.path = "~/Content/Images/Projects/";
                    picture.ProjectDetail = projectDetail;
                    break;
                case "education":
                    var education = db.Education.Find(picture.EducationViewModelID);
                    picture.path = "~/Content/Images/Education/";
                    picture.Education = education;
                    break;
                case "experience":
                    var experience = db.Experiences.Find(picture.ExperiencesViewModelID);
                    picture.path = "~/Content/Images/Experiences/";
                    picture.Experience = experience;
                    break;
                case "skill":
                    var skill = db.Skills.Find(picture.SkillsViewModelID);
                    picture.path = "~/Content/Images/Skills/";
                    picture.Skill = skill;
                    break;
            }
                        
            picture.FileName = file.FileName;
            file.SaveAs(HttpContext.Server.MapPath(picture.path + file.FileName));
         
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PicturesViewModel projectPicturesViewModel = db.PicturesApp.Find(id);
            if (projectPicturesViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", projectPicturesViewModel.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", projectPicturesViewModel.ExperiencesViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", projectPicturesViewModel.SkillsViewModelID);
            return View(projectPicturesViewModel);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,path,FileName,Description,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID")] PicturesViewModel projectPicturesViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectPicturesViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", projectPicturesViewModel.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", projectPicturesViewModel.ExperiencesViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", projectPicturesViewModel.SkillsViewModelID);
            return View(projectPicturesViewModel);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PicturesViewModel projectPicturesViewModel = db.PicturesApp.Find(id);
            if (projectPicturesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectPicturesViewModel);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PicturesViewModel projectPicturesViewModel = db.PicturesApp.Find(id);
            db.PicturesApp.Remove(projectPicturesViewModel);
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
