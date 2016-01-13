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
    [Authorize]
    public class PicturesOldController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Pictures
        public ActionResult Index()
        {
            var picturesProject = db.PicturesApp.Include(p => p.ProjectDetail);
            var see = picturesProject.ToList();
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
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject");
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,path,FileName,Description,ProjectDetailsViewModelID")] PicturesViewModel projectPicturesViewModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null)
            {
                var picture = projectPicturesViewModel;
                var projectDetail = db.DetailsProject.Find(picture.ProjectDetailsViewModelID);
                
                picture.path = "~/Content/Images/Projects/";
                picture.FileName = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath(picture.path + file.FileName));

                picture.ProjectDetail = projectDetail;
                db.PicturesApp.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
            return View(projectPicturesViewModel);
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
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
            return View(projectPicturesViewModel);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,path,FileName,Description,ProjectDetailsViewModelID")] PicturesViewModel projectPicturesViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectPicturesViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectDetailsViewModelID = new SelectList(db.DetailsProject, "ID", "Subject", projectPicturesViewModel.ProjectDetailsViewModelID);
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
