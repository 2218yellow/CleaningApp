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
    public class GardenAssignsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GardenAssigns
        public ActionResult Index()
        {
            return View(db.GardenAssigns.ToList());
        }

        // GET: GardenAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GardenAssign gardenAssign = db.GardenAssigns.Find(id);
            if (gardenAssign == null)
            {
                return HttpNotFound();
            }
            return View(gardenAssign);
        }

        // GET: GardenAssigns/Create
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
            ViewBag.Order_ID = new SelectList(db.GardeningOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address");
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today).Where(s => s.service.SerName == "Gardening"), "EmpID", "EmpName");
            return View();
        }

        // POST: GardenAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GardenOrderId,EmpID,Order_ID")] GardenAssign gardenAssign)
        {
            if (ModelState.IsValid)
            {
                gardenAssign.GetName();
                gardenAssign.EmployeeStatus();
                gardenAssign.EmployeeDate();
                db.GardenAssigns.Add(gardenAssign);
                db.SaveChanges();
                Email objmail = new Email();

                objmail.SendConfirmation(gardenAssign.GetEmail(), gardenAssign.GetCustomerName(), gardenAssign.GetServiceName(), gardenAssign.GetServiceDate(), gardenAssign.GetEmployeeName());
                return RedirectToAction("Create");
            }
            ViewBag.Order_ID = new SelectList(db.GardeningOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", gardenAssign.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", gardenAssign.EmpID);
        

            return View(gardenAssign);
        }

        // GET: GardenAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GardenAssign gardenAssign = db.GardenAssigns.Find(id);
            if (gardenAssign == null)
            {
                return HttpNotFound();
            }
            ViewBag.Order_ID = new SelectList(db.GardeningOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", gardenAssign.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", gardenAssign.EmpID);
            return View(gardenAssign);
        }

        // POST: GardenAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GardenOrderId,EmpID,Order_ID")] GardenAssign gardenAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gardenAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_ID = new SelectList(db.GardeningOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", gardenAssign.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", gardenAssign.EmpID);
            return View(gardenAssign);
        }

        // GET: GardenAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GardenAssign gardenAssign = db.GardenAssigns.Find(id);
            if (gardenAssign == null)
            {
                return HttpNotFound();
            }
            return View(gardenAssign);
        }

        // POST: GardenAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GardenAssign gardenAssign = db.GardenAssigns.Find(id);
            db.GardenAssigns.Remove(gardenAssign);
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
