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
    public class EducationOldController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Education
        public ActionResult Index()
        {
            return View(db.Education.ToList());
        }

        // GET: Education
        [AllowAnonymous]        
        public ActionResult _Index()
        {
            return View(db.Education.ToList());
        }
        // GET: Education/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationViewModel educationViewModel = db.Education.Find(id);
            if (educationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(educationViewModel);
        }

        // GET: Education/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Education/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SchoolName,YearGraduate,NbYearsToGraduate")] EducationViewModel educationViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Education.Add(educationViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(educationViewModel);
        }

        // GET: Education/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationViewModel educationViewModel = db.Education.Find(id);
            if (educationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(educationViewModel);
        }

        // POST: Education/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SchoolName,YearGraduate,NbYearsToGraduate")] EducationViewModel educationViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(educationViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(educationViewModel);
        }

        // GET: Education/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationViewModel educationViewModel = db.Education.Find(id);
            if (educationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(educationViewModel);
        }

        // POST: Education/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EducationViewModel educationViewModel = db.Education.Find(id);
            db.Education.Remove(educationViewModel);
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
