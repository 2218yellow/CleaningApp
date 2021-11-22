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
    public class FumigationAssignsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FumigationAssigns
        public ActionResult Index()
        {
            return View(db.FumigationAssigns.ToList());
        }

        // GET: FumigationAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FumigationAssign fumigationAssign = db.FumigationAssigns.Find(id);
            if (fumigationAssign == null)
            {
                return HttpNotFound();
            }
            return View(fumigationAssign);
        }

        // GET: FumigationAssigns/Create
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
            ViewBag.Order_ID = new SelectList(db.FumigationOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address");
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today).Where(s => s.service.SerName == "Fumigation"), "EmpID", "EmpName");
            return View();
        }

        // POST: FumigationAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FumigationAssignId,EmpID,Order_ID")] FumigationAssign fumigationAssign)
        {
            if (ModelState.IsValid)
            {
                fumigationAssign.GetName();
                fumigationAssign.EmployeeStatus();
                fumigationAssign.EmployeeDate();
                db.FumigationAssigns.Add(fumigationAssign);
                db.SaveChanges();
                Email objmail = new Email();

                objmail.SendConfirmation(fumigationAssign.GetEmail(), fumigationAssign.GetCustomerName(), fumigationAssign.GetServiceName(), fumigationAssign.GetServiceDate(), fumigationAssign.GetEmployeeName());
                return RedirectToAction("Create");
            }
            ViewBag.Order_ID = new SelectList(db.FumigationOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", fumigationAssign.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", fumigationAssign.EmpID);
            return View(fumigationAssign);

           
        }

        // GET: FumigationAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FumigationAssign fumigationAssign = db.FumigationAssigns.Find(id);
            if (fumigationAssign == null)
            {
                return HttpNotFound();
            }
            return View(fumigationAssign);
        }

        // POST: FumigationAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FumigationAssignId,EmpID,Order_ID")] FumigationAssign fumigationAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fumigationAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_ID = new SelectList(db.FumigationOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", fumigationAssign.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", fumigationAssign.EmpID);
            return View(fumigationAssign);
        }

        // GET: FumigationAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FumigationAssign fumigationAssign = db.FumigationAssigns.Find(id);
            if (fumigationAssign == null)
            {
                return HttpNotFound();
            }
            return View(fumigationAssign);
        }

        // POST: FumigationAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FumigationAssign fumigationAssign = db.FumigationAssigns.Find(id);
            db.FumigationAssigns.Remove(fumigationAssign);
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
