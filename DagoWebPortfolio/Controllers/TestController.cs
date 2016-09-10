using DagoWebPortfolio.Models;
using QCBDManagementCommon.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                Log.write("education list count = "+ db.Education.Count(), "WAR");
                return View(db.Education.ToList());
            }
            //return Content("Education page");
        }
        
        public ActionResult _Index()
        {
            //using (var Ddb = new DBDisplayModelContext())
            //{
            //    ViewBag.AvatarUrl = Ddb.DisplayWelcome.OrderByDescending(x=>x.ID).First().Path + Ddb.DisplayWelcome.OrderByDescending(x => x.ID).First().FileName;
            //}
            List<EducationViewModel> educationList = new List<EducationViewModel>();
            try
            {
                //educationList = db.Education.Include("Pictures").ToList();
                //populateEducationWithPicture(educationList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
                return View("Error");
            }
            //return Content("Education page");
            return View(educationList.OrderByDescending(x => x.YearGraduate));
        }

        private void populateEducationWithPicture(List<EducationViewModel> educationList)
        {
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                foreach (var education in educationList)
                {
                    education.Pictures = db.PicturesApp.Where(x => x.EducationViewModelID == education.ID).ToList();

                }
            }
        }

        // GET: Education/Details/5
        public ActionResult Details(int? id)
        {
            EducationViewModel educationViewModel;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                educationViewModel = db.Education.Find(id);
            }

            if (educationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(educationViewModel);
            //return Content("Education page");
        }

        // GET: Education/Create
        public ActionResult Create()
        {
            //return View();
            return Content("Education page");
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
                try
                {
                    using (DBModelPortfolioContext db = new DBModelPortfolioContext())
                    {
                        db.Education.Add(educationViewModel);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }
                return RedirectToAction("Index");
            }

            return View(educationViewModel);
            //return Content("Education page");
        }

        // GET: Education/Edit/5
        public ActionResult Edit(int? id)
        {
            EducationViewModel educationViewModel;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                educationViewModel = db.Education.Find(id);
            }

            if (educationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(educationViewModel);
            //return Content("Education page");
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
                try
                {
                    using (DBModelPortfolioContext db = new DBModelPortfolioContext())
                    {
                        db.Entry(educationViewModel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }
                return RedirectToAction("Index");
            }
            return View(educationViewModel);
            //return Content("Education page");
        }

        // GET: Education/Delete/5
        public ActionResult Delete(int? id)
        {
            EducationViewModel educationViewModel;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                educationViewModel = db.Education.Find(id);
            }
            if (educationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(educationViewModel);
            //return Content("Education page");
        }

        // POST: Education/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (DBModelPortfolioContext db = new DBModelPortfolioContext())
                {
                    EducationViewModel educationViewModel = db.Education.Find(id);
                    db.Education.Remove(educationViewModel);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return RedirectToAction("Index");
            //return Content("Education page");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (DBModelPortfolioContext db = new DBModelPortfolioContext())
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
