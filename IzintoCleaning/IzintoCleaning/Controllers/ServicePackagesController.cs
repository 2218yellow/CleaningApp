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
    public class ServicePackagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServicePackages
        public ActionResult Index()
        {
            var servicePackages = db.ServicePackages.Include(s => s.service);
            return View(servicePackages.ToList());
        }

        // GET: ServicePackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackages servicePackages = db.ServicePackages.Find(id);
            if (servicePackages == null)
            {
                return HttpNotFound();
            }
            return View(servicePackages);
        }

        // GET: ServicePackages/Create
        public ActionResult Create()
        {
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName");
            return View();
        }

        // POST: ServicePackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackCode,PackName,PackCost,SerCode")] ServicePackages servicePackages)
        {
            if (ModelState.IsValid)
            {
                db.ServicePackages.Add(servicePackages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", servicePackages.SerCode);
            return View(servicePackages);
        }

        // GET: ServicePackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackages servicePackages = db.ServicePackages.Find(id);
            if (servicePackages == null)
            {
                return HttpNotFound();
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", servicePackages.SerCode);
            return View(servicePackages);
        }

        // POST: ServicePackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackCode,PackName,PackCost,SerCode")] ServicePackages servicePackages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePackages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", servicePackages.SerCode);
            return View(servicePackages);
        }

        // GET: ServicePackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackages servicePackages = db.ServicePackages.Find(id);
            if (servicePackages == null)
            {
                return HttpNotFound();
            }
            return View(servicePackages);
        }

        // POST: ServicePackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicePackages servicePackages = db.ServicePackages.Find(id);
            db.ServicePackages.Remove(servicePackages);
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
