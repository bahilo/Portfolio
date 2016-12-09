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
    public class ContactsController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();

        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactViewModel contactViewModel = db.Contacts.Find(id);
            if (contactViewModel == null)
            {
                return HttpNotFound();
            }
            return View(contactViewModel);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Company,Email,Phone,Comments")] ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contactViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactViewModel);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactViewModel contactViewModel = db.Contacts.Find(id);
            if (contactViewModel == null)
            {
                return HttpNotFound();
            }
            return View(contactViewModel);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Company,Email,Phone,Comments")] ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactViewModel);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactViewModel contactViewModel = db.Contacts.Find(id);
            if (contactViewModel == null)
            {
                return HttpNotFound();
            }
            return View(contactViewModel);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactViewModel contactViewModel = db.Contacts.Find(id);
            db.Contacts.Remove(contactViewModel);
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
