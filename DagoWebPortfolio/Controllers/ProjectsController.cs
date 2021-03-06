﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using System.Data.Entity.Validation;
using DagoWebPortfolio.Interfaces;
using QCBDManagementCommon.Classes;
using System.Globalization;
using DagoWebPortfolio.Classes;

namespace DagoWebPortfolio.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private IProjectsRepository ProjectRepository;
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        private string _culture;
        private string _cultureDefault;

        public ProjectsController(IProjectsRepository rep)
        {
            _culture = CultureInfo.CurrentCulture.Name.Split('-').First();
            _cultureDefault = "en";
            ProjectRepository = rep;
            ProjectRepository.setContext(db);
        }

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects
        [AllowAnonymous]
        public ActionResult _Index()
        {
            var skills = db.Skills.Include(x => x.Projects).Include(x => x.Experiences).Include(x => x.Pictures).ToList();
            ProjectRepository.populateProjectsWithProjectdetails(skills);
            ProjectRepository.populateProjectsWithPicture(skills);
            return View(ProjectRepository.getProjectsOrderByDate(skills));
        }

        // GET: Projects/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectsViewModel projectsViewModel = new ProjectsViewModel();
            try
            {
                projectsViewModel = db.Projects.Where(x => x.ID == id)
                    .Include(x=>x.Summaries)
                    .Include(x => x.ProjectDetail)
                    .Include(x=>x.ProjectDetail.Descriptions)
                    .Include(x => x.ProjectDetail.Pictures).SingleOrDefault();
                var pictures = db.PicturesApp.Include(x => x.ProjectDetail).Where(x => x.ProjectDetail.ID == projectsViewModel.ProjectDetail.ID).ToList();
                projectsViewModel.ProjectDetail.Pictures = pictures;

                // get the detail description based on the user browser language
                projectsViewModel.ProjectDetail.Descriptions = projectsViewModel.ProjectDetail.Descriptions.Where(x => x.Lang.StartsWith(_culture)).ToList();
                
                // get the default description if the user browser language description has not been found
                if (projectsViewModel.ProjectDetail.Descriptions.Count == 0)
                    projectsViewModel.ProjectDetail.Descriptions = projectsViewModel.ProjectDetail.Descriptions.Where(x => x.Lang.StartsWith(_cultureDefault)).ToList();

                // get the project summary based on the user browser language
                projectsViewModel.Summaries = projectsViewModel.Summaries.Where(x => x.Lang.StartsWith(_culture)).ToList();

                // get the default project summary if the user browser language summary has not been found
                if (projectsViewModel.Summaries.Count == 0)
                    projectsViewModel.Summaries = projectsViewModel.Summaries.Where(x => x.Lang.StartsWith(_cultureDefault)).ToList();

                // get all techologies used within the project
                projectsViewModel.TechnoEnv = db.TechnoEnv.Include(x=>x.Picture).Where(x => x.ProjectsViewModelID == projectsViewModel.ID).ToList();
               
            }
            catch (Exception ex)
            {
                Log.debug(ex.Message);
                return RedirectToAction("Index");
            }
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
        public ActionResult Create([Bind(Include = "ID,Title,link,ProjectDetail")] ProjectsViewModel projectsViewModel, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    projectsViewModel.Skills = new List<SkillsViewModel>();
                    ProjectRepository.addOrUpdateSkillWithObjects(projectsViewModel, listSkillOfProjectsId, isSkillSelected);

                    db.Projects.Add(projectsViewModel);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }
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
                var dictionary = new Dictionary<string, object>();

                

                try
                {
                    var origineProject = db.Projects.Include(x => x.ProjectDetail).Include(x => x.Skills).Include(x => x.Summaries).Where(x => x.ID == projectsViewModel.ID).Single();
                    
                    dictionary["projectsViewModel"] = projectsViewModel;
                    dictionary["projectDetailID"] = projectDetailID;
                    dictionary["listSkillOfProjectsId"] = listSkillOfProjectsId;
                    dictionary["isSkillSelected"] = isSkillSelected;

                    ProjectRepository.populateDBWithDataFromForm(dictionary, origineProject);
                    db.Entry(origineProject).State = EntityState.Modified;
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
                    Log.write(raise.Message, "ERR");
                    //throw raise;
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }

                return RedirectToAction("Index");
            }
            return View(projectsViewModel);
        }


        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            ProjectsViewModel projectsViewModel = new ProjectsViewModel();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                projectsViewModel = db.Projects.Find(id);
                if (projectsViewModel == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return View(projectsViewModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ProjectRepository.deleteProject(id, Server);
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
