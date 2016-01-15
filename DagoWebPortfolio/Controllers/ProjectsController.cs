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
using DagoWebPortfolio.Models.DisplayViewModel;
using DagoWebPortfolio.Interfaces;

namespace DagoWebPortfolio.Controllers 
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private IProjectsRepository ProjectRepository;
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        public ProjectsController(IProjectsRepository rep)
        {
            ProjectRepository = rep;
        }

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects
        [AllowAnonymous]
        public ActionResult _Index()
        {
            var skills = db.Skills.ToList();
            populateProjectsWithProjectdetails(skills);
            populateProjectsWithPicture(skills);
            return View(getProjectsOrderByDate(skills));
        }

        private IEnumerable<ProjectsViewModel> getProjectsOrderByDate(List<SkillsViewModel> skills)
        {
            var projectsListFromSkills = (from e in (from d in skills select d.Projects.OrderBy(x => x.ProjectDetail.Date)).Distinct() select e).ToList();

            List<ProjectsViewModel> projectsListFinal = new List<ProjectsViewModel>();
            foreach (var projectsList in projectsListFromSkills)
            {
                foreach (var project in projectsList.ToList())
                {
                    if (project != null)
                    {
                        projectsListFinal.Add(project);
                    }
                }
            }
            var projectsListFinalDistict = projectsListFinal.Distinct().Reverse().ToList();

            return projectsListFinalDistict;
        }

        private void populateProjectsWithProjectdetails(List<SkillsViewModel> skills)
        {
            foreach (var skill in skills)
            {
                foreach (var project in skill.Projects)
                {
                    project.ProjectDetail = db.DetailsProject.Where(x => x.ID == project.ProjectDetail.ID).DefaultIfEmpty().Single();
                }
            }
        }

        private void populateProjectsWithPicture(List<SkillsViewModel> skills)
        {
            foreach (var skill in skills)
            {
                foreach (var project in skill.Projects)
                {
                    project.ProjectDetail.Pictures = db.PicturesApp.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID).ToList();
                }
            }
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectsViewModel projectsViewModel = db.Projects.Where(x=>x.ID == id).Include("ProjectDetail").DefaultIfEmpty().Single();
            if (projectsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectsViewModel);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            var project = new ProjectsViewModel();
            project.Skills = db.Skills.ToList();
            return View(project);
        }

        // POST: Projects/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,link,Resume,ProjectDetail")] ProjectsViewModel projectsViewModel, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected)
        {
            if (ModelState.IsValid)
            {
                addOrUpdateSkillWithObjects(projectsViewModel, listSkillOfProjectsId, isSkillSelected);

                db.Projects.Add(projectsViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectsViewModel);
        }


       

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ProjectID = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectsViewModelWithInclude = db.Projects.Include("ProjectDetail");
            var projectsViewModel = projectsViewModelWithInclude.First(x => x.ID == id);
            projectsViewModel.Skills = db.Skills.Include(p => p.Projects).ToList();
            if (projectsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectsViewModel);
        }

        // POST: Projects/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,link,Resume,ProjectDetail")] ProjectsViewModel projectsViewModel, string projectDetailID, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected)
        {
            if (ModelState.IsValid)
            {
                var id = Int32.Parse(projectDetailID);

                addOrUpdateSkillWithObjects(projectsViewModel, listSkillOfProjectsId, isSkillSelected);

                var origineProject = db.Projects.Where(x => x.ID == projectsViewModel.ID).Include(x=>x.ProjectDetail).Include(x=>x.Skills).DefaultIfEmpty().Single();
                                
                origineProject.link = projectsViewModel.link;
                origineProject.Resume = projectsViewModel.Resume;
                origineProject.Title = projectsViewModel.Title;
                origineProject.Skills = projectsViewModel.Skills;

                ProjectDetailsViewModel newProjectDetail = new ProjectDetailsViewModel();
                newProjectDetail.Subject = projectsViewModel.ProjectDetail.Subject;
                newProjectDetail.Status = projectsViewModel.ProjectDetail.Status;
                newProjectDetail.Description = projectsViewModel.ProjectDetail.Description;
                newProjectDetail.Date = projectsViewModel.ProjectDetail.Date;
                newProjectDetail.Client = projectsViewModel.ProjectDetail.Client;

                origineProject.ProjectDetail = newProjectDetail;






                try
                {


                    //db.Entry(origineProject).State = EntityState.Modified;
                    UpdateModel(origineProject,new string[]{ "ID","Title","link","Resume","ProjectDetail" });
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

                    throw raise;
                }


                return RedirectToAction("Index");
            }
            return View(projectsViewModel);
        }



        private void addOrUpdateSkillWithObjects(ProjectsViewModel project, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected)
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
                    newSkill = db.Skills.Where(y => y.ID == id).Include("CategoryViewModel").Include("LevelsViewModel").DefaultIfEmpty().Single();
                    if (isSelected == "true")
                    {
                        project.Skills.Add(newSkill);
                        listAddedSkillsId.Add(id);
                    }
                }
            }

            // removing uncheck Projects skills
            if (listSkillOfProjectsId != null)
            {
                foreach (var skillIdString in listSkillOfProjectsId)
                {
                    var skillId = Int32.Parse(skillIdString);
                    if ((listAddedSkillsId != null && !listAddedSkillsId.Contains(skillId)) || listAddedSkillsId == null)
                    {
                        var skillToRemove = db.Skills.Where(x => x.ID == skillId).Include("CategoryViewModel").Include("LevelsViewModel").First();
                        project.Skills.Remove(skillToRemove);
                    }
                }
            }

        }




        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectsViewModel projectsViewModel = db.Projects.Find(id);
            if (projectsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectsViewModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var projectsViewModelWithIncludes = db.Projects.Include("ProjectDetail");
            var projectsViewModel = projectsViewModelWithIncludes.First(x => x.ID == id);
            var detailWithIncludes = db.DetailsProject.Include("Pictures").Where(x => x.ID == projectsViewModel.ProjectDetail.ID).Single();
            try
            {
                var listPictures = detailWithIncludes.Pictures.Where(x => x.ProjectDetailsViewModelID == projectsViewModel.ProjectDetail.ID).DefaultIfEmpty();

                foreach (var picture in listPictures)
                {
                    deletePicture(picture);
                }
                db.DetailsProject.Remove(db.DetailsProject.First(x => x.ID == projectsViewModel.ProjectDetail.ID));
                db.Projects.Remove(projectsViewModel);
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

                throw raise;
            }
            return RedirectToAction("Index");
        }

        private void deletePicture(PicturesViewModel picture)
        {
            if (picture != null)
            {

                string[] savedFiles;
                string origineFileWithPath;
                
                savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + picture.path));
                origineFileWithPath = Server.MapPath("~" + picture.path) + picture.FileName;

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
    }
}
