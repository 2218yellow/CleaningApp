//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using IzintoCleaning.Models;
//using Microsoft.AspNet.Identity;

//namespace IzintoCleaning.Controllers
//{
//    public class CustomerServicesController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: CustomerServices
//        public ActionResult Index()
//        {

//            if (User.IsInRole("Admin"))
//            {
//                var id = User.Identity.GetUserId();
//                var customerService1 = db.CustomerService.Include(c => c.customer).Include(c => c.ServiceDesc);
//                return View(customerService1.ToList());//.Where(x => x.CustID == id));
//            }
//            var userid = User.Identity.GetUserId();
//            var customerService = db.CustomerService.Include(c => c.customer).Include(c => c.ServiceDesc).Where(x => x.CustID == userid);
//            // var customerService = db.CustomerService.Include(c => c.customer).Include(c => c.ServiceDesc);
//            return View(customerService.ToList());
//        }

//        // GET: CustomerServices/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CustomerService customerService = db.CustomerService.Find(id);
//            if (customerService == null)
//            {
//                return HttpNotFound();
//            }
//            return View(customerService);
//        }

//        // GET: CustomerServices/Create
//        public ActionResult Create()
//        {
//            var userid = User.Identity.GetUserId();
//            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName");
//            ViewBag.DescID = new SelectList(db.ServiceDescs, "DescID", "CritName");
//            ViewBag.EmpID = new SelectList(db.Employees, "EmpID", "EmpName");
//            return View();
//        }

//        [ChildActionOnly]
//        public ActionResult Cleaning()
//        {
//            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName");
//            ViewBag.DescID = new SelectList(db.ServiceDescs, "DescID", "CritName");
//            return PartialView("_Cleaning");
//        }

//        public ActionResult Confirm()
//        {
//            return View();
//        }

//        // POST: CustomerServices/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "TransID,CustID,DescID,SerCost,SerCodeCost,FinalCost,DateOfService,EmpID,empStatus")] CustomerService customerService)
//        {
//            if (customerService.CheckDate() == false)
//            {
//                ViewBag.Err = "Please select a date that has not passed and is not today";
//                return View("Create");
//            }
//            else if (ModelState.IsValid)
//            {
//                string str = Request.Params["btn1"];
//                if (str == "View Final Amount")
//                {
//                    customerService.SerCost = customerService.getSerCost();
//                    customerService.SerCodeCost = customerService.getDescCost();
//                    customerService.FinalCost = customerService.calcFinal();

//                }
//                else if (str == "Confirm Order")
//                {
//                    customerService.SerCost = customerService.getSerCost();
//                    customerService.SerCodeCost = customerService.getDescCost();
//                    customerService.FinalCost = customerService.calcFinal();

//                    db.CustomerService.Add(customerService);
//                    db.SaveChanges();
//                    return RedirectToAction("Confirm");
//                }
//            }

//            var userid = User.Identity.GetUserId();
//            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", customerService.CustID == userid);
//            ViewBag.DescID = new SelectList(db.ServiceDescs, "DescID", "CritName", customerService.DescID);
//            ViewBag.EmpID = new SelectList(db.Employees, "EmpID", "EmpName", customerService.EmpId);

//            return View(customerService);
//        }

//        // GET: CustomerServices/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CustomerService customerService = db.CustomerService.Find(id);
//            Employee employee = db.Employees.Find(id);
//            if (customerService == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", customerService.CustID);
//            ViewBag.DescID = new SelectList(db.ServiceDescs, "DescID", "CritName", customerService.DescID);
//            ViewBag.EmpID = new SelectList(db.Employees, "EmpID", "EmpName", customerService.EmpId);
//            return View(customerService);
//        }

//        // POST: CustomerServices/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "TransID,CustID,DescID,SerCost,SerCodeCost,FinalCost,EmpID,empStatus")] CustomerService customerService)
//        {
//            if (ModelState.IsValid)
//            {
//                string str = Request.Params["btn1"];
//                if (str == "Display")
//                {
//                    customerService.SerCost = customerService.getSerCost();
//                    customerService.SerCodeCost = customerService.getDescCost();
//                    customerService.FinalCost = customerService.calcFinal();
//                    customerService.showStatus = customerService.getAssign();
//                }
//                else if (str == "Assign")
//                {
//                    customerService.SerCost = customerService.getSerCost();
//                    customerService.SerCodeCost = customerService.getDescCost();
//                    customerService.FinalCost = customerService.calcFinal();
//                    customerService.Assign();
//                    // customerService.empStatus = customerService.Assign();
//                    db.Entry(customerService).State = EntityState.Modified;
//                    db.SaveChanges();
//                    return RedirectToAction("Index");
//                }
//            }

//            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", customerService.CustID);
//            ViewBag.DescID = new SelectList(db.ServiceDescs, "DescID", "CritName", customerService.DescID);
//            ViewBag.EmpID = new SelectList(db.Employees, "EmpID", "EmpName", customerService.EmpId);


//            return View(customerService);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Assign([Bind(Include = "EmpID,empStatus")] CustomerService customerService)
//        {
//            if (ModelState.IsValid)
//            {
//                    customerService.Assign();
//                    // customerService.empStatus = customerService.Assign();
//                    db.Entry(customerService).State = EntityState.Modified;
//                    db.SaveChanges();
//                    return RedirectToAction("Index");
//            }
//            ViewBag.EmpID = new SelectList(db.Employees, "EmpID", "EmpName", customerService.EmpId);
//            return View(customerService);
//        }

//        // GET: CustomerServices/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CustomerService customerService = db.CustomerService.Find(id);
//            if (customerService == null)
//            {
//                return HttpNotFound();
//            }
//            return View(customerService);
//        }

//        // POST: CustomerServices/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            CustomerService customerService = db.CustomerService.Find(id);
//            db.CustomerService.Remove(customerService);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
