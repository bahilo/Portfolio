using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;

namespace DagoWebPortfolio.Controllers
{
    [Authorize]
    public class DetailsController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();
               

        // GET: Details
        public ActionResult Index()
        {
            return View(db.DetailsProject.ToList());
        }

        // GET: Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetailsViewModel projectDetailsViewModel = db.DetailsProject.Find(id);
            if (projectDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectDetailsViewModel);
        }

        // GET: Details/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Subject,Status,Date,Client,Description")] ProjectDetailsViewModel projectDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                db.DetailsProject.Add(projectDetailsViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectDetailsViewModel);
        }

        // GET: Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetailsViewModel projectDetailsViewModel = db.DetailsProject.Find(id);
            if (projectDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectDetailsViewModel);
        }

        // POST: Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Subject,Status,Date,Client,Description")] ProjectDetailsViewModel projectDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectDetailsViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectDetailsViewModel);
        }

        // GET: Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetailsViewModel projectDetailsViewModel = db.DetailsProject.Find(id);
            if (projectDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(projectDetailsViewModel);
        }

        // POST: Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectDetailsViewModel projectDetailsViewModel = db.DetailsProject.Find(id);
            db.DetailsProject.Remove(projectDetailsViewModel);
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
