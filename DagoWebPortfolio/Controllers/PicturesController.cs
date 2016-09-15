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
using System.IO;
using QCBDManagementCommon.Classes;
using DagoWebPortfolio.Classes;

namespace DagoWebPortfolio.Controllers
{
    [Authorize]
    public class PicturesController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Pictures
        public ActionResult Index()
        {
            var picturesProjects = db.PicturesApp.Include("Education").Include("Experience").Include("ProjectDetail").Include("Skill").ToList();
            populatePictureWithObjects(picturesProjects);
            return View(picturesProjects);
        }

        private void populatePictureWithObjects(List<PicturesViewModel> pictures)
        {
            foreach (var picture in pictures)
            {
                picture.Skill = db.Skills.Where(x => x.ID == picture.SkillsViewModelID).DefaultIfEmpty().Single();
                picture.Education = db.Education.Where(x => x.ID == picture.EducationViewModelID).DefaultIfEmpty().Single();
                picture.Experience = db.Experiences.Where(x => x.ID == picture.ExperiencesViewModelID).DefaultIfEmpty().Single();
                picture.ProjectDetail = db.DetailsProject.Where(x => x.ID == picture.ProjectDetailsViewModelID).DefaultIfEmpty().Single();

            }
        }


        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PicturesViewModel picturesViewModel = db.PicturesApp.Where(x => x.ID == id).Include("Education").Include("Experience").Include("ProjectDetail").Include("Skill").SingleOrDefault();
            try
            {
                populateSinglePictureWithObjects(picturesViewModel);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
                return View("Error");
            }
            if (picturesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(picturesViewModel);
        }

        private void populateSinglePictureWithObjects(PicturesViewModel picture)
        {
            if (picture != null)
            {
                picture.Skill = db.Skills.Where(x => x.ID == picture.SkillsViewModelID).DefaultIfEmpty().Single();
                picture.Education = db.Education.Where(x => x.ID == picture.EducationViewModelID).DefaultIfEmpty().Single();
                picture.Experience = db.Experiences.Where(x => x.ID == picture.ExperiencesViewModelID).DefaultIfEmpty().Single();
                picture.ProjectDetail = db.DetailsProject.Where(x => x.ID == picture.ProjectDetailsViewModelID).DefaultIfEmpty().Single();
            }
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
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,link,IsAbout,IsWelcome,Description,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID,Skill,ProjectDetail,Education,Experience")] PicturesViewModel projectPicturesViewModel, HttpPostedFileBase file, string link_picture_to)
        {
            if (ModelState.IsValid && file != null)
            {
                try
                {
                    var picture = projectPicturesViewModel;
                    addPictureTable(picture, file, link_picture_to);
                    db.PicturesApp.Add(picture);
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
                    //return View("Error");
                    throw raise;
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                    return View("Error");
                }
                //return Content(Utility.getFileFullPath(Utility.getDirectory(projectPicturesViewModel.path), file.FileName));
                return RedirectToAction("Index");
            }

            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", projectPicturesViewModel.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", projectPicturesViewModel.ExperiencesViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", projectPicturesViewModel.SkillsViewModelID);
            return View(projectPicturesViewModel);
                
        }

        private void addPictureTable(PicturesViewModel picture, HttpPostedFileBase file, string link_picture_to)
        {
            //picture.path = Utility.getDirectory("Content","Images");
            picture.path = "/Content/Images/";
            picture.Education = null;
            picture.ProjectDetail = null;
            picture.Skill = null;
            picture.Experience = null;
            
            //picture.EducationViewModelID = null;
            //picture.ExperiencesViewModelID = null;
            //picture.SkillsViewModelID = null;
            //picture.ProjectDetailsViewModelID = null;

            switch (link_picture_to)
            {
                case "project":
                    var projectDetail = db.DetailsProject.Where(x => x.ID == picture.ProjectDetailsViewModelID).DefaultIfEmpty().Single();
                    picture.path += "Projects/";
                    picture.ProjectDetail = projectDetail;
                    picture.Education = null;
                    picture.EducationViewModelID = null;
                    picture.Experience = null;
                    picture.ExperiencesViewModelID = null;
                    picture.Skill = null;
                    picture.SkillsViewModelID = null;
                    //projectDetail.Pictures.Add(picture);
                    break;
                case "education":
                    var education = db.Education.Find(picture.EducationViewModelID);
                    picture.path += "Education/";
                    picture.Education = education;
                    picture.ProjectDetail = null;
                    picture.ProjectDetailsViewModelID = null;
                    picture.Experience = null;
                    picture.ExperiencesViewModelID = null;
                    picture.Skill = null;
                    picture.SkillsViewModelID = null;
                    //education.Pictures.Add(picture);
                    break;
                case "experience":
                    var experience = db.Experiences.Find(picture.ExperiencesViewModelID);
                    picture.path += "Experiences/";
                    picture.Experience = experience;
                    picture.Education = null;
                    picture.EducationViewModelID = null;
                    picture.ProjectDetail = null;
                    picture.ProjectDetailsViewModelID = null;
                    picture.Skill = null;
                    picture.SkillsViewModelID = null;
                    //experience.Pictures.Add(picture);
                    break;
                case "skill":
                    var skill = db.Skills.Find(picture.SkillsViewModelID);
                    picture.path += "Skills/";
                    picture.Skill = skill;
                    picture.Education = null;
                    picture.EducationViewModelID = null;
                    picture.ProjectDetail = null;
                    picture.ProjectDetailsViewModelID = null;
                    picture.Experience = null;
                    picture.ExperiencesViewModelID = null;
                    break;
                default:
                    picture.EducationViewModelID = null;
                    picture.ExperiencesViewModelID = null;
                    picture.SkillsViewModelID = null;
                    picture.ProjectDetailsViewModelID = null;
                    break;
            }

            if (file != null)
            {
                List<string> pathElementList = picture.path.Split('/').ToList();
                
                //string[] savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + picture.path));
                string[] savedFiles = System.IO.Directory.GetFiles(Utility.getDirectory(pathElementList[0], pathElementList.ToArray()));
                if (!string.IsNullOrEmpty(picture.FileName))
                {
                    pathElementList.Add(picture.FileName);
                    var origineFileWithPath = Utility.getDirectory(pathElementList[0], pathElementList.ToArray()); //Server.MapPath("~" + picture.path) + picture.FileName;

                    foreach (var f in savedFiles)
                    {
                        if (origineFileWithPath.Equals(f))
                            System.IO.File.Delete(f);
                    }
                }
                
                picture.FileName = file.FileName.Replace(" ", "_");
                file.SaveAs(Utility.getDirectory(picture.path, picture.FileName));

                //picture.FileName = file.FileName;
                //file.SaveAs(HttpContext.Server.MapPath(picture.path + file.FileName));
            }

            
            
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PicturesViewModel picturesViewModel = db.PicturesApp.Find(id);
            if (picturesViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", picturesViewModel.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", picturesViewModel.ExperiencesViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", picturesViewModel.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", picturesViewModel.SkillsViewModelID);
            return View(picturesViewModel);
        }

        // POST: Pictures/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,IsAbout,IsWelcome,link,Description,ProjectDetailsViewModelID,EducationViewModelID,ExperiencesViewModelID,SkillsViewModelID")] PicturesViewModel picturesViewModel, HttpPostedFileBase file, string link_picture_to, string fileName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    picturesViewModel.FileName = fileName;
                    addPictureTable(picturesViewModel, file, link_picture_to);
                    //updatePictureTable(picturesViewModel, file, link_picture_to, fileName);
                    db.Entry(picturesViewModel).State = EntityState.Modified;
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
                    //return View("Error");
                    throw raise;
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                    return View("Error");
                }


                return RedirectToAction("Index");
            }
            ViewBag.EducationViewModelID = new SelectList(db.Education, "ID", "SchoolName", picturesViewModel.EducationViewModelID);
            ViewBag.ExperiencesViewModelID = new SelectList(db.Experiences, "ID", "Title", picturesViewModel.ExperiencesViewModelID);
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", picturesViewModel.ProjectDetailsViewModelID);
            ViewBag.SkillsViewModelID = new SelectList(db.Skills, "ID", "Title", picturesViewModel.SkillsViewModelID);
            return View(picturesViewModel);
        }
        

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PicturesViewModel picturesViewModel = db.PicturesApp.Find(id);
            if (picturesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(picturesViewModel);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PicturesViewModel projectPicturesViewModel = db.PicturesApp.Find(id);

            projectPicturesViewModel.Education = null;
            projectPicturesViewModel.EducationViewModelID = null;
            projectPicturesViewModel.ProjectDetail = null;
            projectPicturesViewModel.ProjectDetailsViewModelID = null;
            projectPicturesViewModel.Experience = null;
            projectPicturesViewModel.ExperiencesViewModelID = null;
            projectPicturesViewModel.Skill = null;
            projectPicturesViewModel.SkillsViewModelID = null;

            string[] savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + projectPicturesViewModel.path));
            var origineFileWithPath = Server.MapPath("~" + projectPicturesViewModel.path) + projectPicturesViewModel.FileName;

            foreach (var f in savedFiles)
            {
                if (origineFileWithPath.Equals(f))
                    System.IO.File.Delete(f);
            }

            try
            {
                db.PicturesApp.Remove(projectPicturesViewModel);
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
