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
    public class EmployeeOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmployeeOrders
        public ActionResult Index()
        {
          
            return View();
        }

        // GET: EmployeeOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeOrder employeeOrder = db.EmployeeOrders.Find(id);
            if (employeeOrder == null)
            {
                return HttpNotFound();
            }
            return View(employeeOrder);
        }

        // GET: EmployeeOrders/Create
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
            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a => a.EmpmloyeeName == null).Where(x=>x.paymentstatus==true).Where(x=>x.Confirm==false), "Order_ID", "address");
            ViewBag.EmpID = new SelectList(db.Employees.Where(s=>s.Status=="Available").Where(d => d.Today != DateTime.Today).Where(s => s.service.SerName == "Cleaning"), "EmpID", "EmpName");
            return View();
        }

        // POST: EmployeeOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeOrderId,EmpID,Order_ID")] EmployeeOrder employeeOrder)
        {
            if (ModelState.IsValid)
            {
                employeeOrder.GetName();
                employeeOrder.EmployeeStatus();
                employeeOrder.EmployeeDate();
                employeeOrder.GetConfirm();
                db.EmployeeOrders.Add(employeeOrder);
                db.SaveChanges();
                Email objmail = new Email();

                objmail.SendConfirmation(employeeOrder.GetEmail(), employeeOrder.GetCustomerName(), employeeOrder.GetServiceName(), employeeOrder.GetServiceDate(), employeeOrder.GetEmployeeName());
                return RedirectToAction("Create");
            }

            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a=>a.EmpmloyeeName==null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", employeeOrder.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d=>d.Today!=DateTime.Today), "EmpID", "EmpName", employeeOrder.EmpID);
            return View(employeeOrder);
        }

        // GET: EmployeeOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeOrder employeeOrder = db.EmployeeOrders.Find(id);
            if (employeeOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", employeeOrder.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", employeeOrder.EmpID);
            return View(employeeOrder);
        }

        // POST: EmployeeOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeOrderId,EmpID,Order_ID")] EmployeeOrder employeeOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_ID = new SelectList(db.CleaningOrders.Where(a => a.EmpmloyeeName == null).Where(x => x.paymentstatus == true).Where(x => x.Confirm == false), "Order_ID", "address", employeeOrder.Order_ID);
            ViewBag.EmpID = new SelectList(db.Employees.Where(s => s.Status == "Available").Where(d => d.Today != DateTime.Today), "EmpID", "EmpName", employeeOrder.EmpID);
            return View(employeeOrder);
        }

        // GET: EmployeeOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeOrder employeeOrder = db.EmployeeOrders.Find(id);
            if (employeeOrder == null)
            {
                return HttpNotFound();
            }
            return View(employeeOrder);
        }

        // POST: EmployeeOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeOrder employeeOrder = db.EmployeeOrders.Find(id);
            db.EmployeeOrders.Remove(employeeOrder);
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
