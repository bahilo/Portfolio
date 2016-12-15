using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using System.Data.Entity.Validation;
using QCBDManagementCommon.Classes;

namespace DagoWebPortfolio.Controllers
{
    [Authorize]
    public class SkillsController : Controller
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
            return View(db.Skills.Include("LevelsViewModel").Include("CategoryViewModel").ToList());
        }
        // GET: Skills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SkillsViewModel skillsViewModel = db.Skills.Where(x=>x.ID == id).Include("LevelsViewModel").Include("CategoryViewModel").DefaultIfEmpty().Single();
            if (skillsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(skillsViewModel);
        }

        // GET: Skills/Create
        public ActionResult Create()
        {
            var skill = new SkillsViewModel();
            skill.CategoryViewModel = new SkillsCategoryViewModel();
            skill.LevelsViewModel = new SkillsLevelsViewModel();
            skill.Experiences = db.Experiences.ToList();
            skill.Projects = db.Projects.ToList();
            setSourceDropDownList(new SkillsCategoryViewModel());
            return View(skill);
        }

        // POST: Skills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,CategoryViewModel,LevelsViewModel")] SkillsViewModel skillsViewModel
                                    , IEnumerable<string> listExperiencesOfSkillId, IEnumerable<string> listProjectsOfSkillId
                                    , IEnumerable<string> isExperienceSelected, IEnumerable<string> isProjectSelected)
        {
            if (ModelState.IsValid)
            {       
                try
                {
                    skillsViewModel.Projects = new List<ProjectsViewModel>();
                    skillsViewModel.Experiences = new List<ExperiencesViewModel>();
                    addOrUpdateSkillWithObjects(skillsViewModel, listExperiencesOfSkillId, listProjectsOfSkillId, isExperienceSelected, isProjectSelected);
                    db.Skills.Add(skillsViewModel);
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

            skillsViewModel.Experiences = db.Experiences.ToList();
            skillsViewModel.Projects = db.Projects.ToList();
            return View(skillsViewModel);
        }
        
        // GET: Skills/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.SkillID = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var skillsViewModel = db.Skills.Include("CategoryViewModel").Include("LevelsViewModel").Where(x => x.ID == id).Single();
            skillsViewModel.Experiences = db.Experiences.Include(p=>p.Skills).ToList();
            skillsViewModel.Projects = db.Projects.Include(p => p.Skills).ToList();
            setSourceDropDownList(skillsViewModel.CategoryViewModel);
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
        public ActionResult Edit([Bind(Include = "ID,Title,Description,LevelsViewModel,CategoryViewModel")] SkillsViewModel skillsViewModel
                                    , string categoryID, string levelID, string categoryTitle
                                    , IEnumerable<string> listExperiencesOfSkillId, IEnumerable<string> listProjectsOfSkillId
                                    , IEnumerable<string> isExperienceSelected, IEnumerable<string> isProjectSelected)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var origineSkill = db.Skills.Where(x => x.ID == skillsViewModel.ID).Include("Projects").Include("Experiences").DefaultIfEmpty().Single();
                    origineSkill.Title = skillsViewModel.Title;

                    skillsViewModel.CategoryViewModel.Title = categoryTitle;
                    var id = Int32.Parse(categoryID);
                    var origineParentCateg = db.Categories.Where(x => x.ID == id).Single();
                    origineParentCateg.Title = skillsViewModel.CategoryViewModel.Title;
                    origineParentCateg.Description = skillsViewModel.CategoryViewModel.Description;
                    origineSkill.CategoryViewModel = origineParentCateg;

                    id = Int32.Parse(levelID);
                    var origineParentLevel = db.Level.Where(x => x.ID == id).Single();
                    origineParentLevel.Level = skillsViewModel.LevelsViewModel.Level;
                    origineParentLevel.comments = skillsViewModel.LevelsViewModel.comments;
                    origineSkill.LevelsViewModel = origineParentLevel;

                    //origineSkill.LevelsViewModel = skillsViewModel.LevelsViewModel;
                    //origineSkill.CategoryViewModel = skillsViewModel.CategoryViewModel;
                    //origineSkill.Projects = skillsViewModel.Projects;
                    //origineSkill.Experiences = skillsViewModel.Experiences;
                    
                    //origineSkill.Description = skillsViewModel.Description;
                    
                    addOrUpdateSkillWithObjects(origineSkill, listExperiencesOfSkillId, listProjectsOfSkillId, isExperienceSelected, isProjectSelected);

                    db.Entry(origineSkill).State = EntityState.Modified;
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
            return View(skillsViewModel);
        }

        private void addOrUpdateSkillWithObjects(SkillsViewModel skill
                                    , IEnumerable<string> listExperiencesOfSkillId, IEnumerable<string> listProjectsIdOfSkill
                                    , IEnumerable<string> isExperienceSelected, IEnumerable<string> isProjectSelected)
        {

            // Experience handling
            List<int> listAddedExperiencesId = new List<int>();
            if ( isExperienceSelected != null  )
            {
                int id;
                ExperiencesViewModel newExperience;
                for (int i = 0; i< isExperienceSelected.Count(); i++)
                {                
                    string isSelected =  isExperienceSelected.ElementAt(i).Split('-').ToList()[0];
                    string isSelectedId = isExperienceSelected.ElementAt(i).Split('-').ToList()[1];
                    id = Int32.Parse(isSelectedId);
                    newExperience = db.Experiences.Find(id);

                    if (  isSelected == "true" )
                    {
                        skill.Experiences.Add(newExperience);
                        listAddedExperiencesId.Add(id);
                    }                  
                }
            }

            // removing uncheck Experiences
            if ( listExperiencesOfSkillId != null )
            { 
                foreach (var experienceIdString in listExperiencesOfSkillId)
                {
                    var experienceId = Int32.Parse(experienceIdString);
                    if ((listAddedExperiencesId != null && !listAddedExperiencesId.Contains(experienceId)) || listAddedExperiencesId == null)
                    {
                        var experienceToRemove = db.Experiences.Find(experienceId);
                        skill.Experiences.Remove(experienceToRemove);
                    }
                }
            }


            // Project handling
            List<int> listAddedProjectsId = new List<int>();
            if ( isProjectSelected != null )
            {
                int id;
                ProjectsViewModel newProject;                
                for (int i = 0; i < isProjectSelected.Count(); i++)
                {
                    string isSelected = isProjectSelected.ElementAt(i).Split('-').ToList()[0];
                    string isSelectedId = isProjectSelected.ElementAt(i).Split('-').ToList()[1];
                    id = Int32.Parse(isSelectedId);
                    newProject = db.Projects.Where(y => y.ID == id).Include("ProjectDetail").DefaultIfEmpty().Single();
                    if (  isSelected == "true"  )
                    {                        
                        skill.Projects.Add(newProject);
                        listAddedProjectsId.Add(id);
                    }                    
                }
            }

            // removing uncheck Projects
            if (listProjectsIdOfSkill != null)
            {
                foreach (var projectIdString in listProjectsIdOfSkill)
                {
                    var projectId = Int32.Parse(projectIdString);
                    if ( (listAddedProjectsId != null && !listAddedProjectsId.Contains(projectId)) || listAddedProjectsId == null )
                    {
                        var projectToRemove = db.Projects.Where(x => x.ID == projectId).Include("ProjectDetail").First();
                        skill.Projects.Remove(projectToRemove);
                    }  
                }
            }

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

            var skillsViewModelWithIncludes = db.Skills.Include("CategoryViewModel").Include("LevelsViewModel");
            var skillsViewModel = skillsViewModelWithIncludes.Where(x => x.ID == id).DefaultIfEmpty().First();

            //SkillsViewModel skillsViewModel = db.Skills.Find(id);

            try
            {
                if ( skillsViewModel != null ) { 
                    var picture = db.PicturesApp.Where(x => x.SkillsViewModelID == skillsViewModel.ID).DefaultIfEmpty().First();
                    deletePicture(picture);                

                    db.Categories.Remove(db.Categories.First(x=>x.ID == skillsViewModel.CategoryViewModel.ID));
                    db.Level.Remove(db.Level.First(x => x.ID == skillsViewModel.LevelsViewModel.ID));
                    db.Skills.Remove(skillsViewModel);
                    db.SaveChanges();
                }
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

        private void deletePicture(PicturesViewModel picture)
        {
            if (picture != null)
            {
                string[] savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + picture.path));
                var origineFileWithPath = Server.MapPath("~" + picture.path) + picture.FileName;

                foreach (var f in savedFiles)
                {
                    if (origineFileWithPath.Equals(f))
                        System.IO.File.Delete(f);
                }
                db.PicturesApp.Remove(picture);
            }
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

        private void setSourceDropDownList(SkillsCategoryViewModel category)
        {
            ViewBag.Categories = new SelectList(new List<string> { "Coding", "Business", "Others", "Certification" }, category.Title);
         }



    }
}
