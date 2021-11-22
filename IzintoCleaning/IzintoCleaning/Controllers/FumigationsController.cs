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
    public class FumigationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fumigations
        public ActionResult Index()
        {

            var fumigations = db.Fumigations.Include(f => f.service);
            return View(fumigations.ToList());
        }

        // GET: Fumigations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fumigation fumigation = db.Fumigations.Find(id);
            if (fumigation == null)
            {
                return HttpNotFound();
            }
            return View(fumigation);
        }

        // GET: Fumigations/Create
        public ActionResult Create()
        {
            ViewBag.SerCode = new SelectList(db.Service.Where(s => s.SerName == "Fumigation"), "SerCode", "SerName");
            return View();
        }

        // POST: Fumigations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Package_ID,SerCode,PackageName,CostArea,CostStorey")] Fumigation fumigation)
        {
            if (ModelState.IsValid)
            {
                db.Fumigations.Add(fumigation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", fumigation.SerCode);
            return View(fumigation);
        }

        // GET: Fumigations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fumigation fumigation = db.Fumigations.Find(id);
            if (fumigation == null)
            {
                return HttpNotFound();
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", fumigation.SerCode);
            return View(fumigation);
        }

        // POST: Fumigations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Package_ID,SerCode,PackageName,CostArea,CostStorey")] Fumigation fumigation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fumigation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SerCode = new SelectList(db.Service.Where(s => s.SerName == "Fumigation"), "SerCode", "SerName", fumigation.SerCode);
            return View(fumigation);
        }

        // GET: Fumigations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fumigation fumigation = db.Fumigations.Find(id);
            if (fumigation == null)
            {
                return HttpNotFound();
            }
            return View(fumigation);
        }

        // POST: Fumigations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fumigation fumigation = db.Fumigations.Find(id);
            db.Fumigations.Remove(fumigation);
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
