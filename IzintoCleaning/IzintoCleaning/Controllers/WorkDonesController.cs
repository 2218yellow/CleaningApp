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
    public class WorkDonesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkDones
        public ActionResult Index()
        {
            return View(db.WorkDones.ToList());
        }

        // GET: WorkDones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkDone workDone = db.WorkDones.Find(id);
            if (workDone == null)
            {
                return HttpNotFound();
            }
            return View(workDone);
        }

        // GET: WorkDones/Create
        public ActionResult Create()
        {
            Employee obj = new Employee();

            foreach (var item in db.Employees)
            {
                if (item.Today != DateTime.Today)
                {
                    item.Status = "Available";
                }
               
            }
            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a => a.EmpmloyeeName == null));
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today).Where(s => s.service.SerName == "Cleaning"), "EmpID", "EmpName");
            return View();
        }


        // POST: WorkDones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkId,EmpID,Order_ID,Status,time")] WorkDone workDone)
        {
            if (ModelState.IsValid)
               
            {
                workDone.GetName();
                workDone.EmployeeStatus();
                workDone.EmployeeDate();
                db.WorkDones.Add(workDone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a => a.EmpmloyeeName == null));
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today).Where(s => s.service.SerName == "Cleaning"), "EmpID", "EmpName");

            return View(workDone);
        }

        // GET: WorkDones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkDone workDone = db.WorkDones.Find(id);
            if (workDone == null)
            {
                return HttpNotFound();
            }
            return View(workDone);
        }

        // POST: WorkDones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkId,EmpID,Order_ID,Status,time")] WorkDone workDone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workDone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a => a.EmpmloyeeName == null));
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today).Where(s => s.service.SerName == "Cleaning"), "EmpID", "EmpName");
            return View(workDone);
        }

        // GET: WorkDones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkDone workDone = db.WorkDones.Find(id);
            if (workDone == null)
            {
                return HttpNotFound();
            }
            return View(workDone);
        }

        // POST: WorkDones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkDone workDone = db.WorkDones.Find(id);
            db.WorkDones.Remove(workDone);
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
