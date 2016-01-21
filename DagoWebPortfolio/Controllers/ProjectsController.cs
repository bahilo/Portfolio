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
            ProjectRepository.setContext(db);
        }

        public ActionResult Index(string target)
        {

            ViewBag.Target = target.ToLower() ?? "";
            
            return View(db.Projects.ToList());
        }

        // GET: Projects
        [AllowAnonymous]
        public ActionResult _Index()
        {
            var skills = db.Skills.ToList();
            ProjectRepository.populateProjectsWithProjectdetails(skills);
            ProjectRepository.populateProjectsWithPicture(skills);
            return View(ProjectRepository.getProjectsOrderByDate(skills));
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
                ProjectRepository.addOrUpdateSkillWithObjects(projectsViewModel, listSkillOfProjectsId, isSkillSelected);

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
                var dictionary = new Dictionary<string,object>();

                dictionary["projectsViewModel"] = projectsViewModel;
                dictionary["projectDetailID"] = projectDetailID;
                dictionary["listSkillOfProjectsId"] = listSkillOfProjectsId;
                dictionary["isSkillSelected"] = isSkillSelected;

                var origineProject = db.Projects.Where(x => x.ID == projectsViewModel.ID).Include(x=>x.ProjectDetail).Include(x=>x.Skills).DefaultIfEmpty().Single();

                ProjectRepository.populateDBWithDataFromForm(dictionary, origineProject);

                try
                {
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
            ProjectRepository.deleteProject(id, Server);
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
