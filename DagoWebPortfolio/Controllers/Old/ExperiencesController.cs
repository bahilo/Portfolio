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
    public class ExperiencesOldController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Experiences
        public ActionResult Index()
        {
            return View(db.Experiences.ToList());
        }

        // GET: Experiences
        [AllowAnonymous]
        public ActionResult _Index()
        {
            return View(db.Skills.Include( x => x.Experiences ).Include( p => p.LevelsViewModel ).ToList());
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
            return View();
        }

        // POST: Experiences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Company,Responsabilities")] ExperiencesViewModel experiencesViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Experiences.Add(experiencesViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(experiencesViewModel);
        }

        // GET: Experiences/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Experiences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Company,Responsabilities")] ExperiencesViewModel experiencesViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(experiencesViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(experiencesViewModel);
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
            db.Experiences.Remove(experiencesViewModel);
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
