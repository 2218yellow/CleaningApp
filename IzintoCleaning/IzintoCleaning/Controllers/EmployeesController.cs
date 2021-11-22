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
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        /* public ActionResult Index(/*string selectStatus)
         {
            var vm = new ListAndSearchVM();

             vm.Statuses = new List<SelectListItem> {
                 new SelectListItem { Value = "Available", Text = "Available" },
                 new SelectListItem {Value = "Not Available", Text= "Not Available"}
             };

             var data = db.Employees;
             if(!String.IsNullOrEmpty(selectStatus))
             {
               //  data = data.Where(f => f.Status == selectStatus);
             }
             vm.Employees = data.ToList();

             var employees = db.Employees.Include(e => e.admin).Include(e => e.service);
             return View(employees.ToList());
         }*/

        public ActionResult Index(string searchString, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "avalaibity" : "";
            //ViewBag.AvailabilitySortParm = sortOrder == "Avalaible" ? "Not Available" : "";
            var employee = from s in db.Employees
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                employee = employee.Where(s => s.EmpSurame.Contains(searchString)
                                       || s.EmpName.Contains(searchString)
                                       || s.Status.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employee = employee.OrderByDescending(s => s.EmpSurame);
                    break;
                /*case "avalaibity":
                    employee = employee.OrderByDescending(s => s.Status);
                    break;
                /*case "date_desc":
                    employee = employee.OrderByDescending(s => s.EmployeeStatus);
                    break;*/
                default:
                    employee = employee.OrderBy(s => s.EmpSurame);
                    break;
            }

            return View(employee.ToList());


        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName");
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpID,EmpName,EmpSurame,EmpEmail,EmpPhone,EmpAddress,Wage,AdminID,SerCode,,Status,EmployeeStatus")] Employee employee)
        {
            employee.Wage = employee.getWage();
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Examine")
                {
                    employee.Wage = employee.getWage();

                }
                else if (str == "Confirm")
                {
                    employee.Status = employee.Availabilty();
                    employee.Wage = employee.getWage();

                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", employee.AdminID);
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", employee.SerCode);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", employee.AdminID);
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", employee.SerCode);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpID,EmpName,EmpSurame,EmpEmail,EmpPhone,EmpAddress,Wage,AdminID,SerCode,Status,EmployeeStatus")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Examine")
                {
                    employee.Wage = employee.getWage();

                }
                else if (str == "Confirm")
                {
                    employee.Wage = employee.getWage();
                    employee.Status = employee.Availabilty();
                    
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", employee.AdminID);
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", employee.SerCode);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
