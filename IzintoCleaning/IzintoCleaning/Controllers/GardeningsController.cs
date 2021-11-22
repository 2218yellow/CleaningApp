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
    public class GardeningsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gardenings
        public ActionResult Index()
        {
            var gardenings = db.Gardenings.Include(g => g.service);
            return View(gardenings.ToList());
        }

        // GET: Gardenings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gardening gardening = db.Gardenings.Find(id);
            if (gardening == null)
            {
                return HttpNotFound();
            }
            return View(gardening);
        }

        // GET: Gardenings/Create
        public ActionResult Create()
        {
            ViewBag.SerCode = new SelectList(db.Service.Where(s => s.SerName == "Gardening"), "SerCode", "SerName");
            return View();
        }

        // POST: Gardenings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Package_ID,SerCode,PackageName,CostArea")] Gardening gardening)
        {
            if (ModelState.IsValid)
            {
                db.Gardenings.Add(gardening);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", gardening.SerCode);
            return View(gardening);
        }

        // GET: Gardenings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gardening gardening = db.Gardenings.Find(id);
            if (gardening == null)
            {
                return HttpNotFound();
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", gardening.SerCode);
            return View(gardening);
        }

        // POST: Gardenings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Package_ID,SerCode,PackageName,CostArea")] Gardening gardening)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gardening).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SerCode = new SelectList(db.Service.Where(s => s.SerName == "Gardening"), "SerCode", "SerName", gardening.SerCode);
            return View(gardening);
        }

        // GET: Gardenings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gardening gardening = db.Gardenings.Find(id);
            if (gardening == null)
            {
                return HttpNotFound();
            }
            return View(gardening);
        }

        // POST: Gardenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gardening gardening = db.Gardenings.Find(id);
            db.Gardenings.Remove(gardening);
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
