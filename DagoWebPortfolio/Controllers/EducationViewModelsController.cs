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
    public class EducationViewModelsController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Experiences
        public ActionResult Index()
        {
            return View(db.Education.ToList());
        }

        // GET: Experiences
        [AllowAnonymous]
        public ActionResult _Index()
        {
            //var exp = db.Experiences.Include(d=>d.Pictures);
            var skills = db.Skills.Include("Experiences").Include("LevelsViewModel").Include("CategoryViewModel").ToList();
            try
            {
                populateExperienceWithPicture(skills);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return View(getExperiencesOrderByDate(skills));
        }

        private IEnumerable<ExperiencesViewModel> getExperiencesOrderByDate(List<SkillsViewModel> skills)
        {
            var experiencesListFromSkills = (from e in (from d in skills select d.Experiences).Distinct() select e).ToList();

            List<ExperiencesViewModel> experiencesListFinal = new List<ExperiencesViewModel>();
            foreach (var experiencesList in experiencesListFromSkills)
            {
                foreach (var experience in experiencesList.ToList())
                {
                    if (experience != null)
                    {
                        experiencesListFinal.Add(experience);
                    }
                }
            }
            var experiencesListFinalDistict = experiencesListFinal.Distinct().OrderByDescending(x => x.EndDate).ToList();

            return experiencesListFinalDistict;
        }

        private void populateExperienceWithPicture(List<SkillsViewModel> skills)
        {
            foreach (var skill in skills)
            {
                foreach (var experience in skill.Experiences)
                {
                    experience.Pictures = db.PicturesApp.Where(x => x.ExperiencesViewModelID == experience.ID).ToList();
                }
            }
        }
        // GET: Experiences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExperiencesViewModel experiencesViewModel = db.Experiences.Find(id);
            if (experiencesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(experiencesViewModel);
        }

        // GET: Experiences/Create
        public ActionResult Create()
        {
            var experience = new ExperiencesViewModel();
            experience.Skills = db.Skills.ToList();
            return View(experience);
        }

        // POST: Experiences/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Company,Responsabilities,StartDate,EndDate,link")] ExperiencesViewModel experiencesViewModel, IEnumerable<string> listSkillOfExperiencesId, IEnumerable<string> isSkillSelected)
        {
            if (ModelState.IsValid)
            {
                addOrUpdateSkillWithObjects(experiencesViewModel, listSkillOfExperiencesId, isSkillSelected);

                try
                {
                    db.Experiences.Add(experiencesViewModel);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }
                return RedirectToAction("Index");
            }

            return View(experiencesViewModel);
        }

        // GET: Experiences/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ExperienceID = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExperiencesViewModel experiencesViewModel = db.Experiences.Find(id);
            experiencesViewModel.Skills = db.Skills.Include(p => p.Experiences).ToList();
            if (experiencesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(experiencesViewModel);
        }

        // POST: Experiences/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Company,Responsabilities,StartDate,EndDate,link")] ExperiencesViewModel experiencesViewModel, IEnumerable<string> listSkillOfExperiencesId, IEnumerable<string> isSkillSelected)
        {
            if (ModelState.IsValid)
            {
                addOrUpdateSkillWithObjects(experiencesViewModel, listSkillOfExperiencesId, isSkillSelected);
                var origineExperience = db.Experiences.Where(x => x.ID == experiencesViewModel.ID).DefaultIfEmpty().Single();

                try
                {
                    //origineExperience.Responsabilities = experiencesViewModel.Responsabilities;
                    origineExperience.link = experiencesViewModel.link;
                    origineExperience.Company = experiencesViewModel.Company;
                    origineExperience.Title = experiencesViewModel.Title;
                    origineExperience.Skills = experiencesViewModel.Skills;
                    origineExperience.StartDate = experiencesViewModel.StartDate;
                    origineExperience.EndDate = experiencesViewModel.EndDate;

                    db.Entry(origineExperience).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    ViewBag.ErrorMessage = raise.Message;
                    Log.write(raise.Message, "ERR");
                    return View("Error");
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                    return View("Error");
                }
                return RedirectToAction("Index");
            }
            return View(experiencesViewModel);
        }


        private void addOrUpdateSkillWithObjects(ExperiencesViewModel experience, IEnumerable<string> listSkillOfExperiencesId, IEnumerable<string> isSkillSelected)
        {
            // Project Skills handling
            List<int> listAddedSkillsId = new List<int>();
            if (isSkillSelected != null)
            {
                int id;
                SkillsViewModel newSkill;
                for (int i = 0; i < isSkillSelected.Count(); i++)
                {
                    string isSelected = isSkillSelected.ElementAt(i).Split('-').ToList()[0];
                    string isSelectedId = isSkillSelected.ElementAt(i).Split('-').ToList()[1];
                    id = Int32.Parse(isSelectedId);
                    newSkill = db.Skills.Where(y => y.ID == id).Include("CategoryViewModel").Include("LevelsViewModel").Include(x => x.Projects).Include(x => x.Projects.Select(s => s.ProjectDetail)).Include(x => x.Experiences).DefaultIfEmpty().Single();
                    //var projectWithDetail = db.Projects.Where(p => p.Skills.Where(y => y.ID == newSkill.ID).Join(p.ProjectDetail)).Include(z=>z.);

                    if (isSelected == "true")
                    {
                        experience.Skills.Add(newSkill);
                        listAddedSkillsId.Add(id);
                    }
                }
            }

            // removing uncheck Projects skills
            if (listSkillOfExperiencesId != null)
            {
                foreach (var skillIdString in listSkillOfExperiencesId)
                {
                    var skillId = Int32.Parse(skillIdString);
                    if ((listAddedSkillsId != null && !listAddedSkillsId.Contains(skillId)) || listAddedSkillsId == null)
                    {
                        var skillToRemove = db.Skills.Where(x => x.ID == skillId).Include("CategoryViewModel").Include("LevelsViewModel").First();
                        experience.Skills.Remove(skillToRemove);
                    }
                }
            }

        }



        // GET: Experiences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExperiencesViewModel experiencesViewModel = db.Experiences.Find(id);
            if (experiencesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(experiencesViewModel);
        }

        // POST: Experiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExperiencesViewModel experiencesViewModel = db.Experiences.Find(id);
            try
            {
                db.Experiences.Remove(experiencesViewModel);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
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
    }
}
