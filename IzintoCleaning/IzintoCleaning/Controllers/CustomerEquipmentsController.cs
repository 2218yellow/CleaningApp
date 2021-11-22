using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;
using Microsoft.AspNet.Identity;

namespace IzintoCleaning.Controllers
{
    public class CustomerEquipmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerEquipments
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var id = User.Identity.GetUserId();
                var customerequipments = db.customerequipment.Include(c => c.customer).Include(c => c.EquipType);
                return View(customerequipments.ToList());//.Where(x => x.CustID == id));
            }
            var userid = User.Identity.GetUserId();
            var customerequipment = db.customerequipment.Include(c => c.customer).Include(c => c.EquipType).Where(x => x.CustID == userid);
            // var customerequipment = db.customerequipment.Include(c => c.customer).Include(c => c.equipment);
            return View(customerequipment.ToList());
        }

        // GET: CustomerEquipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerEquipment customerEquipment = db.customerequipment.Find(id);
            if (customerEquipment == null)
            {
                return HttpNotFound();
            }
            return View(customerEquipment);
        }

        // GET: CustomerEquipments/Create
        public ActionResult Create()
        {
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName");
            ViewBag.EquipID = new SelectList(db.Equipment, "EquipID", "EquipName");
            return View();
        }

        // POST: CustomerEquipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransID,EquipID,CustID,EquipCost,EquipType,Quantity,FinalCost")] CustomerEquipment customerEquipment)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Calculate")
                {
                    customerEquipment.FinalCost = customerEquipment.calcFinalCost();
                    customerEquipment.EquipCost = customerEquipment.getPrice();
                    customerEquipment.EquipType = customerEquipment.getEquipType();
                    customerEquipment.suppEmail = customerEquipment.getSupplierEmail();
                    customerEquipment.suppName = customerEquipment.getSupplierName();

                }
                else if (str == "Buy")
                {
                    customerEquipment.FinalCost = customerEquipment.calcFinalCost();
                    customerEquipment.EquipCost = customerEquipment.getPrice();
                    customerEquipment.EquipType = customerEquipment.getEquipType();
                    db.customerequipment.Add(customerEquipment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", customerEquipment.CustID);
            ViewBag.EquipID = new SelectList(db.Equipment, "EquipID", "EquipName", customerEquipment.EquipID);
            return View(customerEquipment);
        }

        // GET: CustomerEquipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerEquipment customerEquipment = db.customerequipment.Find(id);
            if (customerEquipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", customerEquipment.CustID);
            ViewBag.EquipID = new SelectList(db.Equipment, "EquipID", "EquipName", customerEquipment.EquipID);
            return View(customerEquipment);
        }

        // POST: CustomerEquipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransID,EquipID,CustID,EquipCost,EquipType,Quantity,FinalCost")] CustomerEquipment customerEquipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerEquipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", customerEquipment.CustID);
            ViewBag.EquipID = new SelectList(db.Equipment, "EquipID", "EquipName", customerEquipment.EquipID);
            return View(customerEquipment);
        }

        // GET: CustomerEquipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerEquipment customerEquipment = db.customerequipment.Find(id);
            if (customerEquipment == null)
            {
                return HttpNotFound();
            }
            return View(customerEquipment);
        }

        // POST: CustomerEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerEquipment customerEquipment = db.customerequipment.Find(id);
            db.customerequipment.Remove(customerEquipment);
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
