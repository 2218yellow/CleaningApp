using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;

namespace IzintoCleaning.Controllers
{
    public class CleaningsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cleanings
        public ActionResult Index()
        {
            var cleanings = db.Cleanings.Include(c => c.service);
            return View(cleanings.ToList());
        }

        // GET: Cleanings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaning cleaning = db.Cleanings.Find(id);
            if (cleaning == null)
            {
                return HttpNotFound();
            }
            return View(cleaning);
        }

        // GET: Cleanings/Create
        public ActionResult Create()
        {
            ViewBag.SerCode = new SelectList(db.Service.Where(s => s.SerName == "Cleaning"), "SerCode", "SerName");
            return View();
        }

        // POST: Cleanings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Package_ID,SerCode,PackageName,CostArea,CostStorey")] Cleaning cleaning)
        {
            if (ModelState.IsValid)
            {
                db.Cleanings.Add(cleaning);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", cleaning.SerCode);
            return View(cleaning);
        }

        // GET: Cleanings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaning cleaning = db.Cleanings.Find(id);
            if (cleaning == null)
            {
                return HttpNotFound();
            }
            ViewBag.SerCode = new SelectList(db.Service.Where(s => s.SerName == "Cleaning"), "SerCode", "SerName", cleaning.SerCode);
            return View(cleaning);
        }

        // POST: Cleanings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Package_ID,SerCode,PackageName,CostArea,CostStorey")] Cleaning cleaning)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cleaning).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", cleaning.SerCode);
            return View(cleaning);
        }

        // GET: Cleanings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaning cleaning = db.Cleanings.Find(id);
            if (cleaning == null)
            {
                return HttpNotFound();
            }
            return View(cleaning);
        }

        // POST: Cleanings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cleaning cleaning = db.Cleanings.Find(id);
            db.Cleanings.Remove(cleaning);
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
