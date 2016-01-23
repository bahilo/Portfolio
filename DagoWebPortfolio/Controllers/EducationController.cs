using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using DagoWebPortfolio.Models.DisplayViewModel;

namespace DagoWebPortfolio.Controllers
{
    [Authorize]
    public class EducationController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Education
        public ActionResult Index()
        {
            return View(db.Education.ToList());
        }

        [AllowAnonymous]
        public ActionResult _Index()
        {
            using (var Ddb = new DBDisplayModelContext())
            {
                ViewBag.AvatarUrl = Ddb.DisplayWelcome.OrderByDescending(x=>x.ID).First().Path + Ddb.DisplayWelcome.OrderByDescending(x => x.ID).First().FileName;
            }
            var educationList = db.Education.Include("Pictures").ToList();
            populateEducationWithPicture(educationList);
            return View(educationList.OrderByDescending(x=>x.YearGraduate));
        }

        private void populateEducationWithPicture(List<EducationViewModel> educationList)
        {
            foreach (var education in educationList)
            {
                education.Pictures = db.PicturesApp.Where(x => x.EducationViewModelID == education.ID).ToList();

            }
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
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SchoolName,YearGraduate,NbYearsToGraduate,link,Description")] EducationViewModel educationViewModel)
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
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SchoolName,YearGraduate,NbYearsToGraduate,link,Description")] EducationViewModel educationViewModel)
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
