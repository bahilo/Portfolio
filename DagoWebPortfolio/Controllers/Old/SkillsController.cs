using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;

namespace DagoWebPorfolio2.Controllers
{
    [Authorize]
    public class SkillsOldController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Skills
        public ActionResult Index()
        {
            return View(db.Skills.ToList());
        }

        [AllowAnonymous]
        public ActionResult _Index()
        {
            var skillsViewModel = db.Skills.Include( p1 => p1.CategoryViewModel ).Include( p2 => p2.LevelsViewModel ).ToList();
            return View(skillsViewModel);
        }

        // GET: Skills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillsViewModel skillsViewModel = db.Skills.Find(id);
            if (skillsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(skillsViewModel);
        }

        // GET: Skills/Create
        public ActionResult Create()
        {
            var skillViewModel = new SkillsViewModel { Projects = db.Projects.ToList(), Experiences = db.Experiences.ToList() };// { Projects = PopulateProjectData(), Experiences = PopulateExperienceData() };
            return View(skillViewModel);
        }


        // POST: Skills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SkillsViewModel skillsViewModel, string[] selectedProjectsID, string[] selectedExperiencesID)
        {
            if (ModelState.IsValid)
            {
                var skill = skillsViewModel;
                skill.Projects = new List<ProjectsViewModel>();
                skill.Experiences = new List<ExperiencesViewModel>();

                AddOrUpdateProjects(skill, selectedProjectsID);
                AddOrUpdateExperiences(skill, selectedExperiencesID);
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skillsViewModel);
        }

        private void AddOrUpdateProjects(SkillsViewModel skill, string[] selectedProjectsID)
        {
            foreach (var selectedProjectId in selectedProjectsID)
            {
                var projectFind = db.Projects.Find(Int32.Parse(selectedProjectId));
                
                //db.Projects.Attach(projectFind);
                skill.Projects.Add(projectFind);
                
            }
        }

        private void AddOrUpdateExperiences(SkillsViewModel skill, string[] selectedExperiencesID)
        {
            foreach (var selectedExperienceId in selectedExperiencesID)
            {
                var experienceFind = db.Experiences.Find(Int32.Parse(selectedExperienceId));

                //db.Experiences.Attach(experienceFind);
                skill.Experiences.Add(experienceFind);

            }
        }
        /*public ActionResult Create([Bind(Include = "ID,Title,Description")] SkillsViewModel skillsViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Skills.Add(skillsViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skillsViewModel);
        }*/

        // GET: Skills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillsViewModel skillsViewModel = db.Skills.Find(id);
            if (skillsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(skillsViewModel);
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description")] SkillsViewModel skillsViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skillsViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skillsViewModel);
        }

        // GET: Skills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillsViewModel skillsViewModel = db.Skills.Find(id);
            if (skillsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(skillsViewModel);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SkillsViewModel skillsViewModel = db.Skills.Find(id);
            db.Skills.Remove(skillsViewModel);
            //db.Skills.RemoveRange(db.Skills.Where(x=>x.CategoryViewModel.ID = CategoryViewModel_ID));
            //db.SaveChanges();
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
